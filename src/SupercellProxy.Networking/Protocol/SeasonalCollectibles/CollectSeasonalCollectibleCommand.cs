using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.SeasonalCollectibles;

/// <summary>Collects the reward from a spawned seasonal gift.</summary>
public sealed record CollectSeasonalCollectibleCommand(int CollectibleId) : CodecCommand<CollectSeasonalCollectibleCommand>, ICommandCodecProvider<CollectSeasonalCollectibleCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectSeasonalCollectibleCommand> Codec { get; } = Codecs.Command(
        type: 128,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.CollectibleId,
            static (collectibleId) => new CollectSeasonalCollectibleCommand(collectibleId)
        )
    );
}
