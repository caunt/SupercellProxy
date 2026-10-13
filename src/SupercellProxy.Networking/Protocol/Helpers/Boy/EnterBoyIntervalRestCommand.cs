using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Helpers.Boy;

/// <summary>Sends the boy into his interval rest.</summary>
public sealed record EnterBoyIntervalRestCommand : CodecCommand<EnterBoyIntervalRestCommand>, ICommandCodecProvider<EnterBoyIntervalRestCommand>
{
    /// <inheritdoc />
    public static CommandCodec<EnterBoyIntervalRestCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new EnterBoyIntervalRestCommand()));
}
