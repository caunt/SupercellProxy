using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Emotes;

/// <summary>Marks the new emotes as seen.</summary>
public sealed record MarkEmotesSeenCommand : CodecCommand<MarkEmotesSeenCommand>, ICommandCodecProvider<MarkEmotesSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkEmotesSeenCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new MarkEmotesSeenCommand()));
}
