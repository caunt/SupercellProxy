using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Records that the edit mode intro was shown once edit mode is unlocked.</summary>
public sealed record MarkEditModeIntroSeenCommand : CodecCommand<MarkEditModeIntroSeenCommand>, ICommandCodecProvider<MarkEditModeIntroSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkEditModeIntroSeenCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new MarkEditModeIntroSeenCommand()));
}
