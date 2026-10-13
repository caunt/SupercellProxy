using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings;

/// <summary>Marks the terms of service notice as seen.</summary>
public sealed record MarkTermsOfServiceSeenCommand : CodecCommand<MarkTermsOfServiceSeenCommand>, ICommandCodecProvider<MarkTermsOfServiceSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkTermsOfServiceSeenCommand> Codec { get; } = Codecs.Command(type: 683, Codecs.Unit(static () => new MarkTermsOfServiceSeenCommand()));
}
