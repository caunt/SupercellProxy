using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Characters;

/// <summary>Collects Angus's accumulated pearls from the pearl bucket.</summary>
public sealed record CollectAngusPearlsCommand(int BucketGlobalId) : CodecCommand<CollectAngusPearlsCommand>, ICommandCodecProvider<CollectAngusPearlsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectAngusPearlsCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BucketGlobalId,
            static (bucketGlobalId) => new CollectAngusPearlsCommand(bucketGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
