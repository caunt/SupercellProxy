using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings;

/// <summary>Marks the tracking confirmation notice as seen.</summary>
public sealed record MarkTrackingConfirmationSeenCommand : CodecCommand<MarkTrackingConfirmationSeenCommand>, ICommandCodecProvider<MarkTrackingConfirmationSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkTrackingConfirmationSeenCommand> Codec { get; } = Codecs.Command(type: 580, Codecs.Unit(static () => new MarkTrackingConfirmationSeenCommand()));
}
