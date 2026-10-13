using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Seen;

/// <summary>Marks the current Valley quests as seen.</summary>
public sealed record MarkMapGameCurrentQuestsSeenCommand : CodecCommand<MarkMapGameCurrentQuestsSeenCommand>, ICommandCodecProvider<MarkMapGameCurrentQuestsSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkMapGameCurrentQuestsSeenCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new MarkMapGameCurrentQuestsSeenCommand()));
}
