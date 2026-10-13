using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Currency;

/// <summary>Claims the free diamonds granted outside a purchase.</summary>
public sealed record ClaimFreeDiamondsCommand : CodecCommand<ClaimFreeDiamondsCommand>, ICommandCodecProvider<ClaimFreeDiamondsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimFreeDiamondsCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new ClaimFreeDiamondsCommand()));
}
