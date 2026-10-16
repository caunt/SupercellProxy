using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Places objects from edit mode storage.</summary>
/// <param name="ObjectGlobalIds">Game object ids of the stored objects to put back on the farm.</param>
public sealed record PlaceEditModeObjectsCommand(ReadOnlyMemory<int> ObjectGlobalIds) : CodecCommand<PlaceEditModeObjectsCommand>, ICommandCodecProvider<PlaceEditModeObjectsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PlaceEditModeObjectsCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Memory(Codecs.VarInt),
            static command => command.ObjectGlobalIds,
            static objectGlobalIds => new PlaceEditModeObjectsCommand(objectGlobalIds)
        ),
        CommandLayout.MetadataFirst
    );
}
