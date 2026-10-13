using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings;

/// <summary>Marks the vouchers hint as seen.</summary>
public sealed record MarkVouchersHintSeenCommand : CodecCommand<MarkVouchersHintSeenCommand>, ICommandCodecProvider<MarkVouchersHintSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkVouchersHintSeenCommand> Codec { get; } = Codecs.Command(type: 572, Codecs.Unit(static () => new MarkVouchersHintSeenCommand()));
}
