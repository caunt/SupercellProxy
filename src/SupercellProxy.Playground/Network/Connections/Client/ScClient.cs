using System.Globalization;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using SupercellProxy.Playground.Commands;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Home;
using SupercellProxy.Playground.Home.Simulation;
using SupercellProxy.Playground.Json;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Configuration;
using SupercellProxy.Playground.Network.Connections.Client.Exceptions;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;
using SupercellProxy.Playground.Network.Protocol;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Connections.Client;

/// <summary>
/// Represents <c>ScClient</c>.
/// </summary>
/// <remarks>
/// Initializes a new <see cref="ScClient"/> instance.
/// </remarks>
public partial class ScClient(ClientConfiguration configuration) : IAsyncDisposable
{
    private readonly ClientConfiguration configuration = configuration;
    private static readonly TimeSpan KeepAliveInterval = TimeSpan.FromSeconds(5);
    private const int HarvestGainDelaySubTicks = 56;
    private const int HarvestCompletionDelaySubTicks = 10;
    private const int ClientTurnIntervalSubTicks = 300;
    private readonly HttpClient _httpClient = new();
    private TcpClient? tcpClient;
    private NetworkStream? _networkStream;
    private MessageStream? _supercellStream;

    /// <summary>
    /// Executes the <c>RunAsync</c> operation.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var loginOkResult = await LoginAsync(cancellationToken).ConfigureAwait(false);

            Console.WriteLine("Logged in.");

            if (loginOkResult.Resources.Length > 0)
                (
                    await GetStreamAsync(cancellationToken).ConfigureAwait(false)
                ).CommandDataResolver = new DataTableResolver(loginOkResult.Resources);

            var keepAliveTask = Task.Run(
                async () =>
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(KeepAliveInterval, TimeProvider.System, cancellationToken)
                            .ConfigureAwait(false);

                        var stream = await GetStreamAsync(cancellationToken).ConfigureAwait(false);
                        await stream
                            .WriteMessageAsync(new KeepAliveMessage(), cancellationToken)
                            .ConfigureAwait(false);
                    }
                },
                cancellationToken
            );

            HandleGoods(loginOkResult.Resources);

            while (!keepAliveTask.IsCompleted)
                await HandleIncomingMessageAsync(cancellationToken).ConfigureAwait(false);

            await keepAliveTask.ConfigureAwait(false);
        }
        catch (LoginException loginException)
        {
            Console.WriteLine($"Login failed: {loginException.Message}");
        }
        catch (EndOfStreamException)
        {
            Console.WriteLine("Connection closed by remote host.");
        }
    }

    /// <summary>
    /// Executes the <c>DisposeAsync</c> operation.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await DisconnectAsync().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gets <c>ReadyFieldsAsync</c>.
    /// </summary>
    public async Task<HarvestField[]> GetReadyFieldsAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var harvest = await OpenGatedHarvestStateAsync(cancellationToken).ConfigureAwait(false);
            await PrepareHarvestStateAsync(harvest, cancellationToken).ConfigureAwait(false);
            return harvest.Executor.GetReadyFields();
        }
        finally
        {
            await DisconnectAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Executes the <c>ValidateSynchronizationAsync</c> operation.
    /// </summary>
    public async Task ValidateSynchronizationAsync(
        int subTick,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var harvest = await OpenGatedHarvestStateAsync(cancellationToken).ConfigureAwait(false);
            await SynchronizeServerCommandsAsync(
                    harvest.Executor,
                    harvest.Stream,
                    harvest.PendingServerCommands,
                    cancellationToken
                )
                .ConfigureAwait(false);

            var homeLoadedTurn = harvest.Executor.ExecuteHomeLoadedCommand();
            await SendTurnAsync(
                    harvest.Executor,
                    harvest.Stream,
                    harvest.PendingServerCommands,
                    homeLoadedTurn,
                    "home-loaded initialization",
                    cancellationToken
                )
                .ConfigureAwait(false);
            await SynchronizeServerCommandsAsync(
                    harvest.Executor,
                    harvest.Stream,
                    harvest.PendingServerCommands,
                    cancellationToken
                )
                .ConfigureAwait(false);

            harvest.State.AdvanceInitialSimulation();

            if (subTick < harvest.State.Tick.SubTick)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(subTick),
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"The first synchronization boundary is sub-tick {harvest.State.Tick.SubTick}."
                    )
                );
            }

            harvest.Executor.AdvanceSimulationTo(subTick);
            await SendTurnAsync(
                    harvest.Executor,
                    harvest.Stream,
                    harvest.PendingServerCommands,
                    harvest.Executor.CreateEmptyTurn(),
                    "synchronization validation",
                    cancellationToken
                )
                .ConfigureAwait(false);
        }
        finally
        {
            await DisconnectAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// <para>Harvests the selected field, or the first genuinely ready field when no global ID is supplied.</para>
    /// </summary>
    public async Task<HarvestResult> HarvestFieldAsync(
        int? fieldGlobalId = null,
        CancellationToken cancellationToken = default
    )
    {
        Console.WriteLine("Loading authoritative home state for harvesting...");
        ScClientPendingHarvest firstHarvest;

        try
        {
            var harvest = await OpenGatedHarvestStateAsync(cancellationToken).ConfigureAwait(false);
            await PrepareHarvestStateAsync(harvest, cancellationToken).ConfigureAwait(false);
            firstHarvest = await ExecuteHarvestAsync(harvest, fieldGlobalId, cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            await DisconnectAsync(cancellationToken).ConfigureAwait(false);
        }

        Console.WriteLine(
            "Restarting ScClient from its saved session to verify the authoritative harvest state..."
        );
        var verificationClient = new ScClient(configuration);
        await using (verificationClient.ConfigureAwait(false))
        {
            var verification = await verificationClient
                .OpenGatedHarvestStateAsync(cancellationToken)
                .ConfigureAwait(false);
            return VerifyHarvest(firstHarvest, verification);
        }
    }

    private static async Task PrepareHarvestStateAsync(
        (
            HarvestState State,
            HarvestExecutor Executor,
            MessageStream Stream,
            Queue<ServerCommand> PendingServerCommands
        ) harvest,
        CancellationToken cancellationToken
    )
    {
        await SynchronizeServerCommandsAsync(
                harvest.Executor,
                harvest.Stream,
                harvest.PendingServerCommands,
                cancellationToken
            )
            .ConfigureAwait(false);

        var homeLoadedTurn = harvest.Executor.ExecuteHomeLoadedCommand();
        await SendTurnAsync(
                harvest.Executor,
                harvest.Stream,
                harvest.PendingServerCommands,
                homeLoadedTurn,
                "home-loaded initialization",
                cancellationToken
            )
            .ConfigureAwait(false);
        await SynchronizeServerCommandsAsync(
                harvest.Executor,
                harvest.Stream,
                harvest.PendingServerCommands,
                cancellationToken
            )
            .ConfigureAwait(false);
        harvest.State.AdvanceInitialSimulation();

        await SendTurnAsync(
                harvest.Executor,
                harvest.Stream,
                harvest.PendingServerCommands,
                harvest.Executor.CreateEmptyTurn(),
                "initial simulation synchronization",
                cancellationToken
            )
            .ConfigureAwait(false);
        await SynchronizeServerCommandsAsync(
                harvest.Executor,
                harvest.Stream,
                harvest.PendingServerCommands,
                cancellationToken
            )
            .ConfigureAwait(false);
    }

    private static async Task<ScClientPendingHarvest> ExecuteHarvestAsync(
        (
            HarvestState State,
            HarvestExecutor Executor,
            MessageStream Stream,
            Queue<ServerCommand> PendingServerCommands
        ) harvest,
        int? fieldGlobalId,
        CancellationToken cancellationToken
    )
    {
        var state = harvest.State;
        var executor = harvest.Executor;
        var field = fieldGlobalId is { } requestedFieldGlobalId
            ? executor.SelectReadyField(requestedFieldGlobalId)
            : executor.SelectReadyField();
        var fieldPositionX = field.GameObject.PositionX;
        var fieldPositionY = field.GameObject.PositionY;
        var crop = field.Data;
        var harvestCount = field.HarvestCount;
        var experienceReward = field.ExperienceReward;
        var cropCountBefore = executor.GetInventoryCount(crop);
        var experienceBefore = executor.GetInventoryCount(state.ExperienceData);

        var startSubTick = state.Tick.SubTick;
        var gainSubTick = checked(startSubTick + HarvestGainDelaySubTicks);
        var completionSubTick = checked(gainSubTick + HarvestCompletionDelaySubTicks);
        var continuationTurnSubTick = checked(startSubTick + ClientTurnIntervalSubTicks);
        var synchronizationSubTick = checked(continuationTurnSubTick + ClientTurnIntervalSubTicks);

        _ = executor.QueueHarvest(field, startSubTick, gainSubTick, completionSubTick);
        await SendHarvestStartAsync(harvest, field.GlobalId, startSubTick, cancellationToken)
            .ConfigureAwait(false);
        await SendHarvestContinuationAsync(
                harvest,
                field.GlobalId,
                gainSubTick,
                completionSubTick,
                continuationTurnSubTick,
                cancellationToken
            )
            .ConfigureAwait(false);
        var synchronizedSubTick = await SendHarvestSynchronizationAsync(
                harvest,
                synchronizationSubTick,
                cancellationToken
            )
            .ConfigureAwait(false);

        return new ScClientPendingHarvest(
            field.GlobalId,
            fieldPositionX,
            fieldPositionY,
            crop,
            harvestCount,
            experienceReward,
            cropCountBefore,
            experienceBefore,
            gainSubTick,
            completionSubTick,
            synchronizedSubTick
        );
    }

    private static async Task SendHarvestStartAsync(
        (
            HarvestState State,
            HarvestExecutor Executor,
            MessageStream Stream,
            Queue<ServerCommand> PendingServerCommands
        ) harvest,
        int fieldGlobalId,
        int startSubTick,
        CancellationToken cancellationToken
    )
    {
        var turn = harvest.Executor.CreateClientCommandTurn();
        EnsureHarvestStartCommand(turn, fieldGlobalId, startSubTick);
        await SendTurnAsync(
                harvest.Executor,
                harvest.Stream,
                harvest.PendingServerCommands,
                turn,
                "harvest start",
                cancellationToken
            )
            .ConfigureAwait(false);
        await SynchronizeServerCommandsAsync(
                harvest.Executor,
                harvest.Stream,
                harvest.PendingServerCommands,
                cancellationToken
            )
            .ConfigureAwait(false);
    }

    private static async Task SendHarvestContinuationAsync(
        (
            HarvestState State,
            HarvestExecutor Executor,
            MessageStream Stream,
            Queue<ServerCommand> PendingServerCommands
        ) harvest,
        int fieldGlobalId,
        int gainSubTick,
        int completionSubTick,
        int continuationTurnSubTick,
        CancellationToken cancellationToken
    )
    {
        harvest.Executor.AdvanceSimulationTo(continuationTurnSubTick);
        var turn = harvest.Executor.CreateClientCommandTurn();
        EnsureHarvestContinuationCommands(
            turn,
            fieldGlobalId,
            gainSubTick,
            completionSubTick,
            continuationTurnSubTick
        );
        await SendTurnAsync(
                harvest.Executor,
                harvest.Stream,
                harvest.PendingServerCommands,
                turn,
                "harvest gain and completion",
                cancellationToken
            )
            .ConfigureAwait(false);
        await SynchronizeServerCommandsAsync(
                harvest.Executor,
                harvest.Stream,
                harvest.PendingServerCommands,
                cancellationToken
            )
            .ConfigureAwait(false);
    }

    private static async Task<int> SendHarvestSynchronizationAsync(
        (
            HarvestState State,
            HarvestExecutor Executor,
            MessageStream Stream,
            Queue<ServerCommand> PendingServerCommands
        ) harvest,
        int synchronizationSubTick,
        CancellationToken cancellationToken
    )
    {
        harvest.Executor.AdvanceSimulationTo(synchronizationSubTick);
        var turn = harvest.Executor.CreateEmptyTurn();
        await SendTurnAsync(
                harvest.Executor,
                harvest.Stream,
                harvest.PendingServerCommands,
                turn,
                "post-harvest synchronization",
                cancellationToken
            )
            .ConfigureAwait(false);
        await SynchronizeServerCommandsAsync(
                harvest.Executor,
                harvest.Stream,
                harvest.PendingServerCommands,
                cancellationToken
            )
            .ConfigureAwait(false);
        return turn.SubTick;
    }

    private static HarvestResult VerifyHarvest(
        ScClientPendingHarvest harvest,
        (
            HarvestState State,
            HarvestExecutor Executor,
            MessageStream Stream,
            Queue<ServerCommand> PendingServerCommands
        ) verification
    )
    {
        var verificationField = verification.State.Fields.Single(candidate =>
            candidate.GameObject.PositionX == harvest.FieldPositionX
            && candidate.GameObject.PositionY == harvest.FieldPositionY
        );
        var cropCountAfter = verification.Executor.GetInventoryCount(harvest.Crop);
        var experienceAfter = verification.Executor.GetInventoryCount(
            verification.State.ExperienceData
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

    private async Task<(
        HarvestState State,
        HarvestExecutor Executor,
        MessageStream Stream,
        Queue<ServerCommand> PendingServerCommands
    )> OpenGatedHarvestStateAsync(CancellationToken cancellationToken)
    {
        var loginOkResult = await LoginAsync(cancellationToken).ConfigureAwait(false);
        var dataTableResolver = new DataTableResolver(loginOkResult.Resources);
        var stream = await GetStreamAsync(cancellationToken).ConfigureAwait(false);
        stream.CommandDataResolver = dataTableResolver;
        var pendingServerCommands = new Queue<ServerCommand>();
        var ownHomeDataMessage = await WaitForKeepAliveAndOwnHomeDataAsync(
                stream,
                pendingServerCommands,
                cancellationToken
            )
            .ConfigureAwait(false);
        var state = HarvestState.Create(ownHomeDataMessage, dataTableResolver);

        return (state, new HarvestExecutor(state), stream, pendingServerCommands);
    }

    private static async Task WaitForKeepAliveAsync(
        MessageStream stream,
        Queue<ServerCommand> pendingServerCommands,
        CancellationToken cancellationToken
    )
    {
        Console.WriteLine("Waiting for a keep-alive round trip before the next turn...");
        await Task.Delay(KeepAliveInterval, TimeProvider.System, cancellationToken)
            .ConfigureAwait(false);
        await stream
            .WriteMessageAsync(new KeepAliveMessage(), cancellationToken)
            .ConfigureAwait(false);

        while (true)
        {
            var message = await stream.ReadMessageAsync(cancellationToken).ConfigureAwait(false);

            if (message is KeepAliveOkMessage)
                break;

            if (message is OutOfSyncMessage)
                throw CreateOutOfSyncException();

            if (message is AvailableServerCommandMessage availableServerCommandMessage)
            {
                if (availableServerCommandMessage.Command is not ServerCommand serverCommand)
                    throw new InvalidDataException(
                        string.Create(
                            CultureInfo.InvariantCulture,
                            $"Available command {availableServerCommandMessage.Command.Type} is not a server command."
                        )
                    );

                pendingServerCommands.Enqueue(serverCommand);
            }

            Console.WriteLine(
                $"Received before keep-alive acknowledgement: {message.GetType().Name}"
            );
        }

        Console.WriteLine("Keep-alive round trip completed.");
    }

    private static async Task SendTurnAsync(
        HarvestExecutor executor,
        MessageStream stream,
        Queue<ServerCommand> pendingServerCommands,
        EndClientTurnMessage turn,
        string description,
        CancellationToken cancellationToken
    )
    {
        ValidateTurnRoundTrip(turn, stream.CommandDataResolver);
        Console.WriteLine(
            string.Create(
                CultureInfo.InvariantCulture,
                $"Sending {description} turn with commands {string.Join(',', turn.Commands.ToArray().Select(static command => command.Type))} at sub-tick {turn.SubTick}..."
            )
        );
        await stream.WriteMessageAsync(turn, cancellationToken).ConfigureAwait(false);
        executor.ConfirmTurnSent(turn);
        await WaitForKeepAliveAsync(stream, pendingServerCommands, cancellationToken)
            .ConfigureAwait(false);
    }

    private static void ValidateTurnRoundTrip(
        EndClientTurnMessage turn,
        ICommandDataResolver? dataResolver
    )
    {
        var id = MessageRegistry.GetId<EndClientTurnMessage>();
        var version = MessageRegistry.GetVersion<EndClientTurnMessage>();
        var container = turn.ToContainer(id, version);
        var payload = container.Payload.ToArray();
        container.Payload.Position = 0;
        var decoded = EndClientTurnMessage.Create(
            container,
            CommandEnvironment.Production,
            dataResolver
        );
        var reencoded = decoded.ToContainer(id, version).Payload.ToArray();

        if (!payload.AsSpan().SequenceEqual(reencoded))
            throw new InvalidDataException(
                $"{nameof(EndClientTurnMessage)} did not round-trip byte-exactly before sending."
            );
    }

    private static void EnsureHarvestStartCommand(
        EndClientTurnMessage turn,
        int fieldGlobalId,
        int startSubTick
    )
    {
        if (turn.SubTick != startSubTick)
        {
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"The harvest start turn is at sub-tick {turn.SubTick}; expected {startSubTick}."
                )
            );
        }

        if (
            turn.Commands.Length is not 1
            || turn.Commands.Span[0] is not StartHarvestFieldCommand start
        )
        {
            throw new InvalidOperationException(
                "The harvest start turn must contain only command 544."
            );
        }

        if (start.FieldGlobalId != fieldGlobalId || start.ExecuteSubTick != startSubTick)
        {
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"The harvest start command does not target field {fieldGlobalId} at sub-tick {startSubTick}."
                )
            );
        }
    }

    private static void EnsureHarvestContinuationCommands(
        EndClientTurnMessage turn,
        int fieldGlobalId,
        int gainSubTick,
        int completionSubTick,
        int turnSubTick
    )
    {
        if (turn.SubTick != turnSubTick)
        {
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"The harvest continuation turn is at sub-tick {turn.SubTick}; expected {turnSubTick}."
                )
            );
        }

        if (
            turn.Commands.Length is not 2
            || turn.Commands.Span[0] is not HarvestFieldGainCommand gain
            || turn.Commands.Span[1] is not HarvestFieldCommand completion
        )
        {
            throw new InvalidOperationException(
                "The harvest continuation turn must contain only commands 657 and 506 in that order."
            );
        }

        if (gain.FieldGlobalId != fieldGlobalId || completion.FieldGlobalId != fieldGlobalId)
        {
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"The harvest continuation commands do not consistently target field {fieldGlobalId}."
                )
            );
        }

        if (gain.ExecuteSubTick != gainSubTick || completion.ExecuteSubTick != completionSubTick)
        {
            throw new InvalidOperationException(
                "The harvest continuation turn does not preserve the scheduled execution sub-ticks."
            );
        }
    }

    private static async Task<int> SynchronizeServerCommandsAsync(
        HarvestExecutor executor,
        MessageStream stream,
        Queue<ServerCommand> pendingServerCommands,
        CancellationToken cancellationToken
    )
    {
        var synchronizedCommandCount = 0;

        while (pendingServerCommands.Count > 0)
        {
            var command = pendingServerCommands.Peek();
            executor.ExecuteServerCommand(command);
            _ = pendingServerCommands.Dequeue();
            synchronizedCommandCount++;

            var turn = executor.CreateServerCommandTurn();
            await SendTurnAsync(
                    executor,
                    stream,
                    pendingServerCommands,
                    turn,
                    "server-command synchronization",
                    cancellationToken
                )
                .ConfigureAwait(false);
        }

        return synchronizedCommandCount;
    }

    private static async Task<OwnHomeDataMessage> WaitForKeepAliveAndOwnHomeDataAsync(
        MessageStream stream,
        Queue<ServerCommand> pendingServerCommands,
        CancellationToken cancellationToken
    )
    {
        Console.WriteLine(
            "Waiting for a keep-alive round trip before loading authoritative home state..."
        );
        await Task.Delay(KeepAliveInterval, TimeProvider.System, cancellationToken)
            .ConfigureAwait(false);
        await stream
            .WriteMessageAsync(new KeepAliveMessage(), cancellationToken)
            .ConfigureAwait(false);

        OwnHomeDataMessage? ownHomeDataMessage = null;
        var keepAliveAcknowledged = false;

        while (!keepAliveAcknowledged || ownHomeDataMessage is null)
        {
            var message = await stream.ReadMessageAsync(cancellationToken).ConfigureAwait(false);

            switch (message)
            {
                case KeepAliveOkMessage:
                    keepAliveAcknowledged = true;
                    break;
                case OwnHomeDataMessage ownHome:
                    ownHomeDataMessage = ownHome;
                    break;
                case OutOfSyncMessage:
                    throw CreateOutOfSyncException();
                case AvailableServerCommandMessage availableServerCommandMessage:
                    if (availableServerCommandMessage.Command is not ServerCommand serverCommand)
                        throw new InvalidDataException(
                            string.Create(
                                CultureInfo.InvariantCulture,
                                $"Available command {availableServerCommandMessage.Command.Type} is not a server command."
                            )
                        );

                    pendingServerCommands.Enqueue(serverCommand);
                    break;
            }

            Console.WriteLine(
                $"Received before gated authoritative home state: {message.GetType().Name}"
            );
        }

        Console.WriteLine(
            "Keep-alive round trip completed before authoritative state initialization."
        );
        return ownHomeDataMessage;
    }

    private static InvalidOperationException CreateOutOfSyncException()
    {
        return new InvalidOperationException(
            "The server rejected the preceding turn as out of sync. "
                + "Server diagnostic state was not included in the exception message."
        );
    }
}
