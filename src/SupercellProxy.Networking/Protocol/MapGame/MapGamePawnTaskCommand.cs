using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// Command 321, carrying one Valley car together with a task collection; the ported client neither sends nor handles it, and
/// what it does is not identified.
/// </summary>
public sealed record MapGamePawnTaskCommand : CodecCommand<MapGamePawnTaskCommand>, ICommandCodecProvider<MapGamePawnTaskCommand>
{
    /// <summary>Initializes a new instance of the <see cref="MapGamePawnTaskCommand"/> record. Creates the command from its decoded fields.</summary>
    /// <param name="pawn">The car the command is about; <see langword="null"/> when not sent.</param>
    /// <param name="taskList">Valley tasks sent with the car; <see langword="null"/> when not sent.</param>
    public MapGamePawnTaskCommand(MapGamePawn? pawn, MapGameTaskList? taskList)
    {
        Pawn = pawn;
        TaskList = taskList;
    }

    /// <inheritdoc />
    public static CommandCodec<MapGamePawnTaskCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.Optional(MapGamePawn.Codec),
            static command => command.Pawn,
            Codecs.Optional(MapGameTaskList.Codec),
            static command => command.TaskList,
            static (pawn, taskList) => new MapGamePawnTaskCommand(pawn, taskList)),
        CommandLayout.MetadataFirst);

    /// <summary>Gets the Valley car the command is about; <see langword="null"/> when not sent.</summary>
    /// <value>The full car record, the first optional field after the command metadata; kept as received because the command's purpose is not identified.</value>
    public MapGamePawn? Pawn { get; }

    /// <summary>Gets the Valley tasks sent with the car; <see langword="null"/> when not sent.</summary>
    /// <value>The task collection that follows <see cref="Pawn"/> on the wire, kept as received for re-encoding.</value>
    public MapGameTaskList? TaskList { get; }
}
