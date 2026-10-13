using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Helpers.Boy;

/// <summary>Claims the delivery boy's box.</summary>
public sealed record ClaimBoyBoxCommand : CodecCommand<ClaimBoyBoxCommand>, ICommandCodecProvider<ClaimBoyBoxCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimBoyBoxCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new ClaimBoyBoxCommand()));
}
