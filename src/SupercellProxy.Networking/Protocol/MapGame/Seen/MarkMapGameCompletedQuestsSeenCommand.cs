using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Seen;

/// <summary>Marks the completed Valley quests as seen.</summary>
public sealed record MarkMapGameCompletedQuestsSeenCommand : CodecCommand<MarkMapGameCompletedQuestsSeenCommand>, ICommandCodecProvider<MarkMapGameCompletedQuestsSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkMapGameCompletedQuestsSeenCommand> Codec { get; } = Codecs.Command(type: 526, Codecs.Unit(static () => new MarkMapGameCompletedQuestsSeenCommand()));
}
