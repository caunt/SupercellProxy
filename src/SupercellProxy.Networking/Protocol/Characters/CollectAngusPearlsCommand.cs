using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Characters;

/// <summary>Collects Angus's accumulated pearls from the pearl bucket.</summary>
/// <param name="BucketGlobalId">The game object id of the pearl bucket on Angus's dock in the fishing area, written as a VarInt.</param>
public sealed record CollectAngusPearlsCommand(int BucketGlobalId) : CodecCommand<CollectAngusPearlsCommand>, ICommandCodecProvider<CollectAngusPearlsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectAngusPearlsCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.BucketGlobalId, static bucketGlobalId => new CollectAngusPearlsCommand(bucketGlobalId)),
        CommandLayout.MetadataFirst);
}
