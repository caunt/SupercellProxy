using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Collects the retained drops of a completed fishing net or trap.</summary>
public sealed record CollectFishingTrapCommand(int SpotId) : CodecCommand<CollectFishingTrapCommand>, ICommandCodecProvider<CollectFishingTrapCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectFishingTrapCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.SpotId, static id => new CollectFishingTrapCommand(id)));
}
