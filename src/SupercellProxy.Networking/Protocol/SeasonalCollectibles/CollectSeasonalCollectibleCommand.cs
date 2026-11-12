using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.SeasonalCollectibles;

/// <summary>Collects the reward from a spawned seasonal gift.</summary>
/// <param name="CollectibleId">Global id of the spawned gift object on the farm (an object id, not a data id), written as a var-int.</param>
public sealed record CollectSeasonalCollectibleCommand(int CollectibleId) : CodecCommand<CollectSeasonalCollectibleCommand>, ICommandCodecProvider<CollectSeasonalCollectibleCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectSeasonalCollectibleCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.CollectibleId,
        static collectibleId => new CollectSeasonalCollectibleCommand(collectibleId)));
}
