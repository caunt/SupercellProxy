using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.DecorationEvents;

/// <summary>Marks the decoration vote tooltip as seen.</summary>
public sealed record MarkDecorationVoteTooltipSeenCommand : CodecCommand<MarkDecorationVoteTooltipSeenCommand>, ICommandCodecProvider<MarkDecorationVoteTooltipSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkDecorationVoteTooltipSeenCommand> Codec { get; } = Codecs.Command(type: 571, Codecs.Unit(static () => new MarkDecorationVoteTooltipSeenCommand()));
}
