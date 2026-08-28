using System.Globalization;
using SupercellProxy.Playground.Commands;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Home.Checksum;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Messages.Serverbound;

namespace SupercellProxy.Playground.Home;

internal sealed class HomeCommands(HomeState state)
{
    private const int HarvestInventoryIndex = 0;
    private readonly HomeState _state = state;
    private readonly List<(Command Command, int SubTick)> _scheduledClientCommands = [];

    public int ScheduledCommandCount => _scheduledClientCommands.Count;

    public FieldState SelectReadyField()
    {
        return _state.Fields.FirstOrDefault(static field =>
                field.IsHarvestReady && !field.IsHarvestStarted && !field.IsHarvestGainApplied
            )
            ?? throw new InvalidOperationException(
                "The authoritative home state contains no ready crop field."
            );
    }

    public FieldState SelectReadyField(int fieldGlobalId)
    {
        var field =
            _state.Fields.FirstOrDefault(field => field.GlobalId == fieldGlobalId)
            ?? throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {fieldGlobalId} is not part of the authoritative home state."
                )
            );

        if (!field.IsHarvestReady || field.IsHarvestStarted || field.IsHarvestGainApplied)
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
        return _state
            .Fields.Where(static field =>
                field.IsHarvestReady && !field.IsHarvestStarted && !field.IsHarvestGainApplied
            )
            .Select(static field => new HarvestField(
                field.GlobalId,
                field.CropData,
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
        _state.CommandExecution.MarkExecuted(
            new StartHarvestFieldCommand(field.GlobalId, _state.Tick.SubTick)
        );
        return CreateTurn();
    }

    public EndClientTurnMessage ExecuteGain(FieldState field)
    {
        EnsureOwnedField(field);
        EnsureNoPendingCommands("harvest gain");

        field.ApplyHarvestGain();
        _state.CommandExecution.MarkExecuted(
            new HarvestFieldGainCommand(field.GlobalId, _state.Tick.SubTick)
        );

        return CreateTurn();
    }

    public EndClientTurnMessage ExecuteCompletion(FieldState field)
    {
        EnsureOwnedField(field);
        EnsureNoPendingCommands("harvest completion");

        _state.ReplaceHarvestedField(field);
        _state.CommandExecution.MarkExecuted(
            new HarvestFieldCommand(field.GlobalId, _state.Tick.SubTick)
        );

        return CreateTurn();
    }

    public EndClientTurnMessage ExecuteHarvest(FieldState field)
    {
        EnsureOwnedField(field);
        EnsureNoPendingCommands("harvest");

        StartHarvest(field);
        _state.CommandExecution.MarkExecuted(
            new StartHarvestFieldCommand(field.GlobalId, _state.Tick.SubTick)
        );
        field.ApplyHarvestGain();
        _state.CommandExecution.MarkExecuted(
            new HarvestFieldGainCommand(field.GlobalId, _state.Tick.SubTick)
        );

        _state.ReplaceHarvestedField(field);
        _state.CommandExecution.MarkExecuted(
            new HarvestFieldCommand(field.GlobalId, _state.Tick.SubTick)
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

        if (!field.IsHarvestReady || field.IsHarvestStarted || field.IsHarvestGainApplied)
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {field.GlobalId} is not ready to queue for harvesting."
                )
            );

        if (
            startSubTick < _state.Tick.SubTick
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
            _scheduledClientCommands.Exists(scheduled =>
                scheduled.Command switch
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
            new StartHarvestFieldCommand(field.GlobalId),
            new HarvestFieldGainCommand(field.GlobalId),
            new HarvestFieldCommand(field.GlobalId),
        ];

        QueueClientCommand(commands[0], startSubTick);
        QueueClientCommand(commands[1], gainSubTick);
        QueueClientCommand(commands[2], completionSubTick);

        return commands;
    }

    private void QueueClientCommand(Command command, int subTick)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (subTick < _state.Tick.SubTick)
        {
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Cannot queue command {command.Type} for past sub-tick {subTick}; "
                )
                    + string.Create(
                        CultureInfo.InvariantCulture,
                        $"the simulation is at {_state.Tick.SubTick}."
                    )
            );
        }

        var insertionIndex = _scheduledClientCommands.FindIndex(queued => queued.SubTick > subTick);

        if (insertionIndex < 0)
            _scheduledClientCommands.Add((command, subTick));
        else
            _scheduledClientCommands.Insert(insertionIndex, (command, subTick));

        ExecuteScheduledClientCommands();
    }

    public void AdvanceSimulationTo(int subTick)
    {
        if (subTick < _state.Tick.SubTick)
            throw new ArgumentOutOfRangeException(
                nameof(subTick),
                "Cannot rewind the authoritative simulation."
            );

        ExecuteScheduledClientCommands();

        while (_state.Tick.SubTick < subTick)
        {
            _state.AdvanceSimulationSubTick();
            ExecuteScheduledClientCommands();
        }
    }

    public EndClientTurnMessage ExecuteHomeLoadedCommand()
    {
        EnsureNoPendingCommands("home-loaded command");
        _state.CommandExecution.MarkExecuted(
            new CommandWithNoFields(CommandRegistry.HomeLoadedCommandType, _state.Tick.SubTick)
        );
        return CreateTurn();
    }

    public void ExecuteServerCommand(ServerCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        ServerCommand executedCommand = command switch
        {
            ServerCommand210 shopCommand
                when shopCommand.Unknown1 == _state.ClientAvatar.HomeId
                    && uint.CreateTruncating(shopCommand.Unknown0)
                        < _state.ClientAvatar.RoadsideShop.Length => ExecuteRoadsideShopCommand(
                shopCommand
            ),
            ServerCommand355 shopEventCommand => ExecuteShopEventCommand(shopEventCommand),
            RoadsideSaleServerCommand saleCommand => ExecuteRoadsideSaleCommand(saleCommand),
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

        _state.CommandExecution.MarkExecuted(executedCommand);
    }

    public void ExecuteClientCommand(Command command)
    {
        ArgumentNullException.ThrowIfNull(command);

        switch (command)
        {
            case CompleteTutorialCommand tutorialCommand:
                ExecuteCompleteTutorialCommand(tutorialCommand);
                break;
            case DecorationEventTutorialCommand decorationEventTutorialCommand:
                _state.DecorationEventManager.ApplyTutorialCommand(decorationEventTutorialCommand);
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
                ResolveField(gainCommand.FieldGlobalId).ApplyHarvestGain();
                break;
            case HarvestFieldCommand completionCommand:
                _state.ReplaceHarvestedField(ResolveField(completionCommand.FieldGlobalId));
                break;
            case PostmanStateCommand:
                _state.Postman.ApplyStateCommand();
                break;
            case RoadsideReceiptCommand roadsideReceiptCommand:
                ExecuteRoadsideReceiptCommand(roadsideReceiptCommand);
                break;
            case NewEventBoardEventSeenCommand eventSeenCommand:
                ExecuteNewEventBoardEventSeenCommand(eventSeenCommand);
                break;
            default:
                throw new NotSupportedException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"Client command {command.Type} execution is not implemented."
                    )
                );
        }

        command.SetExecutionPhaseCounter(_state.Tick.SubTick);
        _state.CommandExecution.MarkExecuted(command);
    }

    private void ExecuteCompleteTutorialCommand(CompleteTutorialCommand command)
    {
        if (
            !_state.DataTableResolver.TryResolve(command.TutorialGlobalId, out var tutorial)
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

    private void ExecuteRoadsideReceiptCommand(RoadsideReceiptCommand command)
    {
        if (uint.CreateTruncating(command.ReceiptIndex) >= _state.ClientAvatar.RoadsideShop.Length)
            return;

        var entry = _state.ClientAvatar.RoadsideShop[command.ReceiptIndex];
        if (!entry.IsSold || entry.BuyerId is null)
            return;

        if (
            !_state.DataTableResolver.TryResolve(GameAssetFiles.Money, "FreeCash", out var freeCash)
            || !_state.DataTableResolver.TryResolve(
                GameAssetFiles.Money,
                "RoadsideShopSoldGoodsValue",
                out var soldGoodsValue
            )
        )
            throw new InvalidDataException("Roadside receipt reward data is unavailable.");

        _state.Inventory.Add(HarvestInventoryIndex, freeCash, entry.Price);
        _state.Inventory.Add(HarvestInventoryIndex, soldGoodsValue, entry.Price);
        _state.ClientAvatar.RoadsideShop[command.ReceiptIndex] = new RoadsideShopEntry(
            BuyerId: null,
            IsSold: false,
            0,
            0,
            0
        );
    }

    private void ExecuteNewEventBoardEventSeenCommand(NewEventBoardEventSeenCommand command)
    {
        if (
            !_state.ChronosEventManager.TryMarkEventSeen(command.EventId, out _, out var cashReward)
        )
            return;

        if (
            !_state.DataTableResolver.TryResolve(GameAssetFiles.Money, "FreeCash", out var freeCash)
        )
            throw new InvalidDataException("Event-board seen reward data is unavailable.");

        _state.Inventory.Add(HarvestInventoryIndex, freeCash, cashReward);
    }

    private void ExecuteMoveGameObjectCommand(MoveGameObjectCommand command)
    {
        var gameObject =
            _state.GameObjects.FirstOrDefault(candidate =>
                candidate.GlobalId == command.GameObjectGlobalId
            )
            ?? throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Cannot move unknown game object {command.GameObjectGlobalId}."
                )
            );

        if (command.ObjectTableId <= 1 || command.ObjectTableId >= _state.HighestDataTableId)
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
            _state.GameObjects.FirstOrDefault(candidate =>
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

        var gathererHabitat = _state.GathererHabitats.FirstOrDefault(candidate =>
            ReferenceEquals(candidate.GameObject, gameObject)
        );

        if (gathererHabitat is null)
            return;

        var gathererDataIds = _state
            .GathererNests.Where(nest =>
                nest.GameObject.Data.GlobalId == gathererHabitat.NestData.GlobalId
            )
            .Select(static nest => nest.GathererData.GlobalId)
            .ToHashSet();

        foreach (
            var gatherer in _state.Gatherers.Where(gatherer =>
                gathererDataIds.Contains(gatherer.GameObject.Data.GlobalId)
            )
        )
        {
            gatherer.GameObject.MoveBy(-command.OffsetX, -command.OffsetY);
        }
    }

    private FieldState ResolveField(int fieldGlobalId)
    {
        return _state.Fields.FirstOrDefault(field => field.GlobalId == fieldGlobalId)
            ?? throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {fieldGlobalId} is not part of the authoritative home state."
                )
            );
    }

    private void StartHarvest(FieldState field)
    {
        var cropData = field.CropData;
        var harvestCount = field.HarvestCount;
        var experienceReward = field.ExperienceReward;

        _state.Inventory.ValidateAdd(HarvestInventoryIndex, cropData, harvestCount);
        _state.Inventory.ValidateAdd(
            HarvestInventoryIndex,
            _state.ExperienceData,
            experienceReward
        );
        field.StartHarvest();
        _state.Inventory.Add(HarvestInventoryIndex, cropData, harvestCount);
        _state.Inventory.Add(HarvestInventoryIndex, _state.ExperienceData, experienceReward);
    }

    private ServerCommand210 ExecuteRoadsideShopCommand(ServerCommand210 command)
    {
        var entry = _state.ClientAvatar.RoadsideShop[command.Unknown0];

        if (entry.Price > 0)
        {
            if (!_state.DataTableResolver.TryResolve(GameAssetFiles.Money, "Cash", out var cash))
                throw new InvalidDataException("Unable to resolve Cash from data/money.csv.");

            _state.Inventory.Add(HarvestInventoryIndex, cash, entry.Price);
        }

        _state.ClientAvatar.RoadsideShop[command.Unknown0] = new RoadsideShopEntry(
            BuyerId: null,
            IsSold: false,
            0,
            0,
            0
        );

        return new ServerCommand210(
            command.Unknown0,
            command.Unknown1,
            command.ServerCommandId,
            _state.Tick.SubTick
        );
    }

    private RoadsideSaleServerCommand ExecuteRoadsideSaleCommand(RoadsideSaleServerCommand command)
    {
        if (
            command.RoadsideOwnerAvatarId != _state.ClientAvatar.HomeId
            || uint.CreateTruncating(command.SlotIndex) >= _state.ClientAvatar.RoadsideShop.Length
        )
            return WithCurrentPhase(command);

        var entry = _state.ClientAvatar.RoadsideShop[command.SlotIndex];
        if (
            entry.ItemGlobalId != command.ItemGlobalId
            || entry.Quantity != command.Quantity
            || entry.Price != command.Price
            || entry.BuyerId == command.BuyerAvatarId
        )
            return WithCurrentPhase(command);

        _state.ClientAvatar.RoadsideShop[command.SlotIndex] = entry with
        {
            BuyerId = command.BuyerAvatarId,
        };
        return WithCurrentPhase(command);

        RoadsideSaleServerCommand WithCurrentPhase(RoadsideSaleServerCommand source) =>
            new(
                source.BuyerAvatarId,
                source.RoadsideOwnerAvatarId,
                source.ItemGlobalId,
                source.SlotIndex,
                source.Price,
                source.Quantity,
                source.ServerCommandId,
                _state.Tick.SubTick
            );
    }

    private ServerCommand355 ExecuteShopEventCommand(ServerCommand355 command)
    {
        _state.ShopEventManager.Apply(command.ShopEventCollection);
        var consumedShopEvents = command.ShopEventCollection is null
            ? null
            : command.ShopEventCollection with
            {
                Events = Memory<ShopEvent>.Empty,
            };

        return new ServerCommand355(
            consumedShopEvents,
            command.ServerCommandId,
            _state.Tick.SubTick
        );
    }

    public int GetInventoryCount(DataTableReference data)
    {
        return _state.Inventory.TryGetValue(HarvestInventoryIndex, data, out var count) ? count : 0;
    }

    public EndClientTurnMessage CreateEmptyTurn()
    {
        EnsureNoPendingCommands("empty turn");
        return CreateTurn();
    }

    public EndClientTurnMessage CreateServerCommandTurn()
    {
        if (_state.CommandExecution.PendingCommandCount is 0)
            throw new InvalidOperationException(
                "Cannot create a server-command turn without pending commands."
            );

        return CreateTurn();
    }

    public EndClientTurnMessage CreateClientCommandTurn()
    {
        if (_state.CommandExecution.PendingCommandCount is 0)
            throw new InvalidOperationException(
                "Cannot create a client-command turn without pending commands."
            );

        if (
            _state
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
        _state.CommandExecution.MarkSent(turn.Commands.Span);
    }

    private EndClientTurnMessage CreateTurn()
    {
        var checksum = GameModeChecksum.Calculate(_state);

        return new EndClientTurnMessage
        {
            Checksum = checksum.Checksum,
            SubTick = _state.Tick.SubTick,
            SubChecksums = checksum.SubChecksums,
            Commands = _state.CommandExecution.GetPendingCommands(),
            Environment = CommandEnvironment.Production,
        };
    }

    private void EnsureOwnedField(FieldState field)
    {
        if (!_state.Fields.Contains(field))
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {field.GlobalId} is not part of the authoritative home state."
                )
            );
    }

    private void EnsureNoPendingCommands(string operation)
    {
        if (_state.CommandExecution.PendingCommandCount is not 0)
            throw new InvalidOperationException(
                $"Cannot create a {operation} turn while commands are pending."
            );
    }

    private void ExecuteScheduledClientCommands()
    {
        while (
            _scheduledClientCommands.Count > 0
            && _scheduledClientCommands[0].SubTick <= _state.Tick.SubTick
        )
        {
            var command = _scheduledClientCommands[0].Command;
            _scheduledClientCommands.RemoveAt(0);
            ExecuteClientCommand(command);
        }
    }
}
