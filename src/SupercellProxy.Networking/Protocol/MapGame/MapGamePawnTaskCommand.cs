using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// <para>Logic command 321. The native class and semantic field names are not present in the stripped client.</para>
/// </summary>
public sealed record MapGamePawnTaskCommand : CodecCommand<MapGamePawnTaskCommand>, ICommandCodecProvider<MapGamePawnTaskCommand>
{
    /// <summary>
    /// Initializes a new <see cref="MapGamePawnTaskCommand"/> instance.
    /// </summary>
    public MapGamePawnTaskCommand(MapGamePawn? pawn, MapGameTaskCollection? taskCollection)
    {
        Pawn = pawn;
        TaskCollection = taskCollection;
    }

    /// <inheritdoc />
    public static CommandCodec<MapGamePawnTaskCommand> Codec { get; } = Codecs.Command(
        type: 321,
        Codecs.Composite(
            Codecs.Optional(MapGamePawn.Codec),
            static command => command.Pawn,
            Codecs.Optional(MapGameTaskCollection.Codec),
            static command => command.TaskCollection,
            static (pawn, taskCollection) => new MapGamePawnTaskCommand(pawn, taskCollection)
        ),
        CommandLayout.MetadataFirst
    );

    /// <summary>
    /// Gets the <c language="csharp">Pawn</c> value.
    /// </summary>
    public MapGamePawn? Pawn { get; }

    /// <summary>
    /// Gets the <c language="csharp">TaskCollection</c> value.
    /// </summary>
    public MapGameTaskCollection? TaskCollection { get; }
}
