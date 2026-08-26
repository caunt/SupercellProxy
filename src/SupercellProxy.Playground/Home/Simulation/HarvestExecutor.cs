using System.Globalization;
using SupercellProxy.Playground.Commands;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Home;
using SupercellProxy.Playground.Home.Checksum;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;

namespace SupercellProxy.Playground.Home.Simulation;

internal sealed class HarvestExecutor(HarvestState state)
{
    private const int HarvestInventoryIndex = 0;
    private readonly HarvestState state = state;
    private readonly List<Command> scheduledClientCommands = [];

    public int ScheduledCommandCount => scheduledClientCommands.Count;

    public FieldState SelectReadyField()
    {
        return state.Fields.FirstOrDefault(static field =>
                field.IsHarvestReady && !field.IsHarvestStarted && !field.IsHarvestGainCompleted
            )
            ?? throw new InvalidOperationException(
                "The authoritative home state contains no ready crop field."
            );
    }

    public FieldState SelectReadyField(int fieldGlobalId)
    {
        var field =
            state.Fields.FirstOrDefault(field => field.GlobalId == fieldGlobalId)
            ?? throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {fieldGlobalId} is not part of the authoritative home state."
                )
            );

        if (!field.IsHarvestReady || field.IsHarvestStarted || field.IsHarvestGainCompleted)
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {fieldGlobalId} is not ready to harvest."
                )
            );

        return field;
    }

    public HarvestField[] GetReadyFields()
    {
        return state
            .Fields.Where(static field =>
                field.IsHarvestReady && !field.IsHarvestStarted && !field.IsHarvestGainCompleted
            )
            .Select(static field => new HarvestField(
                field.GlobalId,
                field.Data,
                field.HarvestCount,
                field.ExperienceReward
            ))
            .ToArray();
    }

    public EndClientTurnMessage ExecuteStart(FieldState field)
    {
        EnsureOwnedField(field);
        EnsureNoPendingCommands("harvest start");
        StartHarvest(field);
        state.CommandExecution.MarkExecuted(
            new StartHarvestFieldCommand(field.GlobalId, state.Tick.SubTick)
        );
        return CreateTurn();
    }

    public EndClientTurnMessage ExecuteGain(FieldState field)
    {
        EnsureOwnedField(field);
        EnsureNoPendingCommands("harvest gain");

        field.CompleteGain();
        state.CommandExecution.MarkExecuted(
            new HarvestFieldGainCommand(field.GlobalId, state.Tick.SubTick)
        );

        return CreateTurn();
    }

    public EndClientTurnMessage ExecuteCompletion(FieldState field)
    {
        EnsureOwnedField(field);
        EnsureNoPendingCommands("harvest completion");

        state.ReplaceHarvestedField(field);
        state.CommandExecution.MarkExecuted(
            new HarvestFieldCommand(field.GlobalId, state.Tick.SubTick)
        );

        return CreateTurn();
    }

    public EndClientTurnMessage ExecuteHarvest(FieldState field)
    {
        EnsureOwnedField(field);
        EnsureNoPendingCommands("harvest");

        StartHarvest(field);
        state.CommandExecution.MarkExecuted(
            new StartHarvestFieldCommand(field.GlobalId, state.Tick.SubTick)
        );
        field.CompleteGain();
        state.CommandExecution.MarkExecuted(
            new HarvestFieldGainCommand(field.GlobalId, state.Tick.SubTick)
        );

        state.ReplaceHarvestedField(field);
        state.CommandExecution.MarkExecuted(
            new HarvestFieldCommand(field.GlobalId, state.Tick.SubTick)
        );

        return CreateTurn();
    }

    public Command[] QueueHarvest(
        FieldState field,
        int startSubTick,
        int gainSubTick,
        int completionSubTick
    )
    {
        EnsureOwnedField(field);

        if (!field.IsHarvestReady || field.IsHarvestStarted || field.IsHarvestGainCompleted)
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {field.GlobalId} is not ready to queue for harvesting."
                )
            );

        if (
            startSubTick < state.Tick.SubTick
            || gainSubTick < startSubTick
            || completionSubTick < gainSubTick
        )
        {
            throw new ArgumentOutOfRangeException(
                nameof(startSubTick),
                "Harvest execution sub-ticks must be current-or-future and ordered start, gain, completion."
            );
        }

        if (
            scheduledClientCommands.Exists(command =>
                command switch
                {
                    StartHarvestFieldCommand start => start.FieldGlobalId == field.GlobalId,
                    HarvestFieldGainCommand gain => gain.FieldGlobalId == field.GlobalId,
                    HarvestFieldCommand completion => completion.FieldGlobalId == field.GlobalId,
                    _ => false,
                }
            )
        )
        {
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {field.GlobalId} already has a queued harvest command."
                )
            );
        }

        Command[] commands =
        [
            new StartHarvestFieldCommand(field.GlobalId, startSubTick),
            new HarvestFieldGainCommand(field.GlobalId, gainSubTick),
            new HarvestFieldCommand(field.GlobalId, completionSubTick),
        ];

        foreach (var command in commands)
            QueueClientCommand(command);

        return commands;
    }

    public void QueueClientCommand(Command command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.ExecuteSubTick < state.Tick.SubTick)
        {
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Cannot queue command {command.Type} for past sub-tick {command.ExecuteSubTick}; "
                )
                    + string.Create(
                        CultureInfo.InvariantCulture,
                        $"the simulation is at {state.Tick.SubTick}."
                    )
            );
        }

        var insertionIndex = scheduledClientCommands.FindIndex(queued =>
            queued.ExecuteSubTick > command.ExecuteSubTick
        );

        if (insertionIndex < 0)
            scheduledClientCommands.Add(command);
        else
            scheduledClientCommands.Insert(insertionIndex, command);

        ExecuteScheduledClientCommands();
    }

    public void AdvanceSimulationTo(int subTick)
    {
        if (subTick < state.Tick.SubTick)
            throw new ArgumentOutOfRangeException(
                nameof(subTick),
                "Cannot rewind the authoritative simulation."
            );

        ExecuteScheduledClientCommands();

        while (state.Tick.SubTick < subTick)
        {
            state.AdvanceSimulationSubTick();
            ExecuteScheduledClientCommands();
        }
    }

    public EndClientTurnMessage ExecuteHomeLoadedCommand()
    {
        EnsureNoPendingCommands("home-loaded command");
        state.CommandExecution.MarkExecuted(new CommandWithNoFields(530, state.Tick.SubTick));
        return CreateTurn();
    }

    public void ExecuteServerCommand(ServerCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        ServerCommand executedCommand = command switch
        {
            ServerCommand210 shopCommand
                when shopCommand.Unknown1 == state.ClientAvatar.HomeId
                    && uint.CreateTruncating(shopCommand.Unknown0)
                        < state.ClientAvatar.RoadsideShop.Length => ExecuteRoadsideShopCommand(
                shopCommand
            ),
            ServerCommand355 shopEventCommand => ExecuteShopEventCommand(shopEventCommand),
            ServerCommand210 shopCommand => throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Cannot execute roadside-shop server command {shopCommand.ServerCommandId} for stand {shopCommand.Unknown0} and home {shopCommand.Unknown1}."
                )
            ),
            _ => throw new NotSupportedException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Server command {command.Type} execution is not implemented."
                )
            ),
        };

        state.CommandExecution.MarkExecuted(executedCommand);
    }

    public void ExecuteClientCommand(Command command)
    {
        ArgumentNullException.ThrowIfNull(command);

        switch (command)
        {
            case CompleteTutorialCommand tutorialCommand:
                ExecuteCompleteTutorialCommand(tutorialCommand);
                break;
            case MoveGameObjectByOffsetCommand moveByOffsetCommand:
                ExecuteMoveGameObjectByOffsetCommand(moveByOffsetCommand);
                break;
            case MoveGameObjectCommand moveCommand:
                ExecuteMoveGameObjectCommand(moveCommand);
                break;
            case StartHarvestFieldCommand startCommand:
                StartHarvest(ResolveField(startCommand.FieldGlobalId));
                break;
            case HarvestFieldGainCommand gainCommand:
                ResolveField(gainCommand.FieldGlobalId).CompleteGain();
                break;
            case HarvestFieldCommand completionCommand:
                state.ReplaceHarvestedField(ResolveField(completionCommand.FieldGlobalId));
                break;
            case PostmanStateCommand:
                state.Postman.ApplyStateCommand();
                break;
            default:
                throw new NotSupportedException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"Client command {command.Type} execution is not implemented."
                    )
                );
        }

        state.CommandExecution.MarkExecuted(command);
    }

    private void ExecuteCompleteTutorialCommand(CompleteTutorialCommand command)
    {
        if (
            !state.DataTableResolver.TryResolve(command.TutorialGlobalId, out var tutorial)
            || tutorial.TableId is not 36
        )
        {
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Cannot complete unknown tutorial data {command.TutorialGlobalId}."
                )
            );
        }
    }

    private void ExecuteMoveGameObjectCommand(MoveGameObjectCommand command)
    {
        var gameObject =
            state.GameObjects.FirstOrDefault(candidate =>
                candidate.GlobalId == command.GameObjectGlobalId
            )
            ?? throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Cannot move unknown game object {command.GameObjectGlobalId}."
                )
            );

        if (command.ObjectTableId <= 1 || command.ObjectTableId >= state.HighestDataTableId)
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Cannot move game object {command.GameObjectGlobalId} with invalid object-table category {command.ObjectTableId}."
                )
            );

        if (gameObject.Data.TableId != command.ObjectTableId)
        {
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Cannot move game object {command.GameObjectGlobalId}: expected object-table category {gameObject.Data.TableId}, received {command.ObjectTableId}."
                )
            );
        }

        gameObject.MoveTo(command.PositionX, command.PositionY);
    }

    private void ExecuteMoveGameObjectByOffsetCommand(MoveGameObjectByOffsetCommand command)
    {
        var gameObject =
            state.GameObjects.FirstOrDefault(candidate =>
                candidate.GlobalId == command.GameObjectGlobalId
            )
            ?? throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Cannot move unknown game object {command.GameObjectGlobalId}."
                )
            );

        if (gameObject.Data.GlobalId != command.ExpectedDataGlobalId)
        {
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Cannot move game object {command.GameObjectGlobalId}: expected data {gameObject.Data.GlobalId}, received {command.ExpectedDataGlobalId}."
                )
            );
        }

        if (
            (gameObject.PositionX >> 9) != command.ExpectedTileX
            || (gameObject.PositionY >> 9) != command.ExpectedTileY
        )
        {
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Cannot move game object {command.GameObjectGlobalId}: its current tile does not match the command's expected tile."
                )
            );
        }

        gameObject.MoveBy(command.OffsetX, command.OffsetY);
        gameObject.SetMirrored(command.Mirrored);

        var gathererHabitat = state.GathererHabitats.FirstOrDefault(candidate =>
            ReferenceEquals(candidate.GameObject, gameObject)
        );

        if (gathererHabitat is null)
            return;

        var gathererDataIds = state
            .GathererNests.Where(nest =>
                nest.GameObject.Data.GlobalId == gathererHabitat.NestData.GlobalId
            )
            .Select(static nest => nest.GathererData.GlobalId)
            .ToHashSet();

        foreach (
            var gatherer in state.Gatherers.Where(gatherer =>
                gathererDataIds.Contains(gatherer.GameObject.Data.GlobalId)
            )
        )
        {
            gatherer.GameObject.MoveBy(-command.OffsetX, -command.OffsetY);
        }
    }

    private FieldState ResolveField(int fieldGlobalId)
    {
        return state.Fields.FirstOrDefault(field => field.GlobalId == fieldGlobalId)
            ?? throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {fieldGlobalId} is not part of the authoritative home state."
                )
            );
    }

    private void StartHarvest(FieldState field)
    {
        var cropData = field.Data;
        var harvestCount = field.HarvestCount;
        var experienceReward = field.ExperienceReward;

        state.Inventory.ValidateAdd(HarvestInventoryIndex, cropData, harvestCount);
        state.Inventory.ValidateAdd(HarvestInventoryIndex, state.ExperienceData, experienceReward);
        field.StartHarvest();
        state.Inventory.Add(HarvestInventoryIndex, cropData, harvestCount);
        state.Inventory.Add(HarvestInventoryIndex, state.ExperienceData, experienceReward);
    }

    private ServerCommand210 ExecuteRoadsideShopCommand(ServerCommand210 command)
    {
        var entry = state.ClientAvatar.RoadsideShop[command.Unknown0];

        if (entry.Price > 0)
        {
            if (!state.DataTableResolver.TryResolve("data/money.csv", "Cash", out var cash))
                throw new InvalidDataException("Unable to resolve Cash from data/money.csv.");

            state.Inventory.Add(HarvestInventoryIndex, cash, entry.Price);
        }

        state.ClientAvatar.RoadsideShop[command.Unknown0] = new RoadsideShopEntry(
            BuyerId: null,
            IsAdvertised: false,
            0,
            0,
            0
        );

        return new ServerCommand210(
            command.Unknown0,
            command.Unknown1,
            command.ServerCommandId,
            state.Tick.SubTick
        );
    }

    private ServerCommand355 ExecuteShopEventCommand(ServerCommand355 command)
    {
        state.ShopEventManager.Apply(command.ShopEvents);
        var consumedShopEvents = command.ShopEvents is null
            ? null
            : command.ShopEvents with
            {
                Events = Memory<ShopEvent>.Empty,
            };

        return new ServerCommand355(
            consumedShopEvents,
            command.ServerCommandId,
            state.Tick.SubTick
        );
    }

    public int GetInventoryCount(DataTableReference data)
    {
        return state.Inventory.TryGetValue(HarvestInventoryIndex, data, out var count) ? count : 0;
    }

    public EndClientTurnMessage CreateEmptyTurn()
    {
        EnsureNoPendingCommands("empty turn");
        return CreateTurn();
    }

    public EndClientTurnMessage CreateServerCommandTurn()
    {
        if (state.CommandExecution.PendingCommandCount is 0)
            throw new InvalidOperationException(
                "Cannot create a server-command turn without pending commands."
            );

        return CreateTurn();
    }

    public EndClientTurnMessage CreateClientCommandTurn()
    {
        if (state.CommandExecution.PendingCommandCount is 0)
            throw new InvalidOperationException(
                "Cannot create a client-command turn without pending commands."
            );

        if (
            state
                .CommandExecution.GetPendingCommands()
                .Any(static command => command is ServerCommand)
        )
            throw new InvalidOperationException(
                "A client-command turn contains a pending server command."
            );

        return CreateTurn();
    }

    public void ConfirmTurnSent(EndClientTurnMessage turn)
    {
        ArgumentNullException.ThrowIfNull(turn);
        state.CommandExecution.MarkSent(turn.Commands.Span);
    }

    private EndClientTurnMessage CreateTurn()
    {
        var checksum = GameModeChecksum.Calculate(state);

        return new EndClientTurnMessage
        {
            Checksum = checksum.Checksum,
            SubTick = state.Tick.SubTick,
            SubChecksums = checksum.SubChecksums,
            Commands = state.CommandExecution.GetPendingCommands(),
            Environment = CommandEnvironment.Production,
        };
    }

    private void EnsureOwnedField(FieldState field)
    {
        if (!state.Fields.Contains(field))
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {field.GlobalId} is not part of the authoritative home state."
                )
            );
    }

    private void EnsureNoPendingCommands(string operation)
    {
        if (state.CommandExecution.PendingCommandCount is not 0)
            throw new InvalidOperationException(
                $"Cannot create a {operation} turn while commands are pending."
            );
    }

    private void ExecuteScheduledClientCommands()
    {
        while (
            scheduledClientCommands.Count > 0
            && scheduledClientCommands[0].ExecuteSubTick <= state.Tick.SubTick
        )
        {
            var command = scheduledClientCommands[0];
            scheduledClientCommands.RemoveAt(0);
            ExecuteClientCommand(command);
        }
    }
}
