using System.Globalization;
using SupercellProxy.Playground.Commands;
using SupercellProxy.Playground.Network.Messages.Serverbound;

namespace SupercellProxy.Playground.Home;

internal sealed class FieldPlots(HomeTurns homeTurns)
{
    private const int ClientTurnIntervalSubTicks = 300;
    private const int HarvestCompletionDelaySubTicks = 10;
    private const int HarvestGainDelaySubTicks = 56;
    private readonly HomeTurns _homeTurns = homeTurns;

    public HarvestField[] GetReady() => _homeTurns.Commands.GetReadyFields();

    public async Task<FieldHarvestVerification> HarvestAsync(
        int? fieldGlobalId,
        Func<EndClientTurnMessage, CancellationToken, Task> sendAsync,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(sendAsync);
        var state = _homeTurns.State;
        var executor = _homeTurns.Commands;
        var field = fieldGlobalId is { } requestedFieldGlobalId
            ? executor.SelectReadyField(requestedFieldGlobalId)
            : executor.SelectReadyField();
        var cropCountBefore = executor.GetInventoryCount(field.CropData);
        var experienceBefore = executor.GetInventoryCount(state.ExperienceData);
        var startSubTick = state.Tick.SubTick;
        var gainSubTick = checked(startSubTick + HarvestGainDelaySubTicks);
        var completionSubTick = checked(gainSubTick + HarvestCompletionDelaySubTicks);
        var continuationSubTick = checked(startSubTick + ClientTurnIntervalSubTicks);
        var synchronizationSubTick = checked(continuationSubTick + ClientTurnIntervalSubTicks);

        _ = executor.QueueHarvest(field, startSubTick, gainSubTick, completionSubTick);
        var startTurn = executor.CreateClientCommandTurn();
        EnsureHarvestStartCommand(startTurn, field.GlobalId, startSubTick);
        await SendAsync(startTurn).ConfigureAwait(false);

        executor.AdvanceSimulationTo(continuationSubTick);
        var continuationTurn = executor.CreateClientCommandTurn();
        EnsureHarvestContinuationCommands(
            continuationTurn,
            field.GlobalId,
            gainSubTick,
            completionSubTick,
            continuationSubTick
        );
        await SendAsync(continuationTurn).ConfigureAwait(false);

        executor.AdvanceSimulationTo(synchronizationSubTick);
        var synchronizationTurn = executor.CreateEmptyTurn();
        await SendAsync(synchronizationTurn).ConfigureAwait(false);

        return new FieldHarvestVerification(
            field.GlobalId,
            field.GameObject.PositionX,
            field.GameObject.PositionY,
            field.CropData,
            field.HarvestCount,
            field.ExperienceReward,
            cropCountBefore,
            experienceBefore,
            gainSubTick,
            completionSubTick,
            synchronizationTurn.SubTick
        );

        async Task SendAsync(EndClientTurnMessage turn)
        {
            await sendAsync(turn, cancellationToken).ConfigureAwait(false);
            _homeTurns.ConfirmTurnSent(turn);
        }
    }

    public static HarvestResult Verify(FieldHarvestVerification harvest, FieldPlots verification)
    {
        ArgumentNullException.ThrowIfNull(harvest);
        ArgumentNullException.ThrowIfNull(verification);
        var verificationField = verification._homeTurns.State.Fields.Single(candidate =>
            candidate.GameObject.PositionX == harvest.FieldPositionX
            && candidate.GameObject.PositionY == harvest.FieldPositionY
        );
        var cropCountAfter = verification._homeTurns.Commands.GetInventoryCount(harvest.Crop);
        var experienceAfter = verification._homeTurns.Commands.GetInventoryCount(
            verification._homeTurns.State.ExperienceData
        );

        if (!verificationField.IsEmpty)
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"The server did not empty harvested field {harvest.FieldGlobalId}."
                )
            );
        if (cropCountAfter != checked(harvest.CropCountBefore + harvest.HarvestCount))
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"The server crop count for {harvest.Crop.Name} changed from {harvest.CropCountBefore} to {cropCountAfter}; expected {harvest.CropCountBefore + harvest.HarvestCount}."
                )
            );
        if (experienceAfter != checked(harvest.ExperienceBefore + harvest.ExperienceReward))
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"The server experience changed from {harvest.ExperienceBefore} to {experienceAfter}; expected {harvest.ExperienceBefore + harvest.ExperienceReward}."
                )
            );

        return new HarvestResult(
            harvest.FieldGlobalId,
            harvest.Crop,
            harvest.CropCountBefore,
            cropCountAfter,
            harvest.ExperienceBefore,
            experienceAfter,
            verificationField.IsEmpty,
            harvest.GainSubTick,
            harvest.CompletionSubTick,
            harvest.SynchronizedSubTick
        );
    }

    private static void EnsureHarvestStartCommand(
        EndClientTurnMessage turn,
        int fieldGlobalId,
        int startSubTick
    )
    {
        if (
            turn.SubTick != startSubTick
            || turn.Commands.Span is not [StartHarvestFieldCommand start]
            || start.FieldGlobalId != fieldGlobalId
            || start.ExecutionPhaseCounter != startSubTick
        )
            throw new InvalidOperationException("The harvest start turn is invalid.");
    }

    private static void EnsureHarvestContinuationCommands(
        EndClientTurnMessage turn,
        int fieldGlobalId,
        int gainSubTick,
        int completionSubTick,
        int turnSubTick
    )
    {
        if (
            turn.SubTick != turnSubTick
            || turn.Commands.Span
                is not [HarvestFieldGainCommand gain, HarvestFieldCommand completion]
            || gain.FieldGlobalId != fieldGlobalId
            || completion.FieldGlobalId != fieldGlobalId
            || gain.ExecutionPhaseCounter != gainSubTick
            || completion.ExecutionPhaseCounter != completionSubTick
        )
            throw new InvalidOperationException("The harvest continuation turn is invalid.");
    }
}
