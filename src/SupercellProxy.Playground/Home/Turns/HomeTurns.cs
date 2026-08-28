using SupercellProxy.Playground.Commands;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;

namespace SupercellProxy.Playground.Home;

internal sealed class HomeTurns(DataTableResolver dataTableResolver)
{
    private readonly Queue<ServerCommand> _availableServerCommands = [];
    private readonly DataTableResolver _dataTableResolver = dataTableResolver;
    private bool _homeInitializationRequested;
    private bool _homeLoaded;
    private bool _initialSynchronizationPending;
    private HomeCommands? _commands;
    private HomeState? _state;

    public bool IsReady =>
        _state is not null
        && _commands is not null
        && _homeLoaded
        && !_initialSynchronizationPending;

    public static bool Handles(IMessage message)
    {
        return message
            is AvailableServerCommandMessage
                or OwnHomeDataMessage
                or OutOfSyncMessage
                or PassthroughMessage { Id: MessageRegistry.HomeInitializationMessageType };
    }

    public EndClientTurnMessage[] Apply(IMessage message)
    {
        switch (message)
        {
            case AvailableServerCommandMessage { Command: ServerCommand command }:
                _availableServerCommands.Enqueue(command);
                break;
            case OwnHomeDataMessage ownHome:
                _state = HomeState.Create(ownHome, _dataTableResolver);
                _commands = new HomeCommands(_state);
                break;
            case PassthroughMessage { Id: MessageRegistry.HomeInitializationMessageType }:
                _homeInitializationRequested = true;
                break;
            case OutOfSyncMessage:
                throw new InvalidOperationException(
                    "The server rejected the preceding turn as out of sync."
                );
            default:
                throw new ArgumentOutOfRangeException(nameof(message));
        }

        return GenerateAutomaticTurns();
    }

    public EndClientTurnMessage GenerateActionTurn(int subTick, ReadOnlySpan<Command> actions)
    {
        if (actions is [{ Type: CommandRegistry.HomeLoadedCommandType }])
        {
            _homeLoaded = true;
            _initialSynchronizationPending = true;
            var homeLoadedTurn = Commands.ExecuteHomeLoadedCommand();
            Commands.ConfirmTurnSent(homeLoadedTurn);
            return homeLoadedTurn;
        }

        AdvanceTo(subTick);

        foreach (var command in actions)
        {
            if (command is ServerCommand)
                throw new InvalidDataException(
                    "A server command was not available to the autonomous home turns."
                );

            Commands.ExecuteClientCommand(command);
        }

        var turn = actions.Length is 0
            ? Commands.CreateEmptyTurn()
            : Commands.CreateClientCommandTurn();
        Commands.ConfirmTurnSent(turn);
        return turn;
    }

    public EndClientTurnMessage CreateSynchronizationTurn(int subTick)
    {
        AdvanceTo(subTick);
        return Commands.CreateEmptyTurn();
    }

    public void ConfirmTurnSent(EndClientTurnMessage turn)
    {
        ArgumentNullException.ThrowIfNull(turn);
        Commands.ConfirmTurnSent(turn);
    }

    private EndClientTurnMessage[] GenerateAutomaticTurns()
    {
        var turns = new List<EndClientTurnMessage>();

        while (TryGenerateAutomaticTurn(out var turn))
        {
            turns.Add(turn);
            Commands.ConfirmTurnSent(turn);
        }

        return turns.ToArray();
    }

    private bool TryGenerateAutomaticTurn(
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out EndClientTurnMessage? turn
    )
    {
        turn = null;

        if (_commands is null || _state is null)
            return false;
        if (_availableServerCommands.TryDequeue(out var command))
        {
            _commands.ExecuteServerCommand(command);
            turn = _commands.CreateServerCommandTurn();
            return true;
        }
        if (_homeInitializationRequested && !_homeLoaded)
        {
            _homeLoaded = true;
            _initialSynchronizationPending = true;
            turn = _commands.ExecuteHomeLoadedCommand();
            return true;
        }
        if (!_initialSynchronizationPending)
            return false;

        _initialSynchronizationPending = false;
        _state.AdvanceInitialSimulation();
        turn = _commands.CreateEmptyTurn();
        return true;
    }

    internal void AdvanceTo(int subTick)
    {
        if (subTick < State.Tick.SubTick)
            throw new InvalidDataException("Client turn rewinds the simulation sub-tick.");
        if (State.Tick.SubTick is 0 && subTick >= 2)
            State.AdvanceInitialSimulation();

        Commands.AdvanceSimulationTo(subTick);
    }

    internal HomeCommands Commands =>
        _commands ?? throw new InvalidDataException("Home turns have no executor.");

    internal HomeState State =>
        _state ?? throw new InvalidDataException("Home turns have no authoritative state.");
}
