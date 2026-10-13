using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Helpers;

/// <summary>Collects the first item waiting in the helper collection area.</summary>
public sealed record CollectHelperAreaCommand : CodecCommand<CollectHelperAreaCommand>, ICommandCodecProvider<CollectHelperAreaCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectHelperAreaCommand> Codec { get; } = Codecs.Command(type: 660, Codecs.Unit(static () => new CollectHelperAreaCommand()));
}
