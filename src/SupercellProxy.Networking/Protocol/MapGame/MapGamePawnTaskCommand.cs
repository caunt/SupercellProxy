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
    /// <summary>Creates the command from its decoded fields.</summary>
    /// <param name="pawn">The car the command is about; null when not sent.</param>
    /// <param name="taskCollection">Valley tasks sent with the car; null when not sent.</param>
    public MapGamePawnTaskCommand(MapGamePawn? pawn, MapGameTaskCollection? taskCollection)
    {
        Pawn = pawn;
        TaskCollection = taskCollection;
    }

    /// <inheritdoc />
    public static CommandCodec<MapGamePawnTaskCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Optional(MapGamePawn.Codec),
            static command => command.Pawn,
            Codecs.Optional(MapGameTaskCollection.Codec),
            static command => command.TaskCollection,
            static (pawn, taskCollection) => new MapGamePawnTaskCommand(pawn, taskCollection)
        ),
        CommandLayout.MetadataFirst
    );

    /// <summary>The Valley car the command is about; null when not sent.</summary>
    public MapGamePawn? Pawn { get; }

    /// <summary>Valley tasks sent with the car; null when not sent.</summary>
    public MapGameTaskCollection? TaskCollection { get; }
}
