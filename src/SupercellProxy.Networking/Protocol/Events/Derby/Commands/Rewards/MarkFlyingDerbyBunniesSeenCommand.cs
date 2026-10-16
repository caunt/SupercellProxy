using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;

/// <summary>Records the number of derby bunny appearances shown to the player.</summary>
public sealed record MarkFlyingDerbyBunniesSeenCommand(int Count) : CodecCommand<MarkFlyingDerbyBunniesSeenCommand>, ICommandCodecProvider<MarkFlyingDerbyBunniesSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkFlyingDerbyBunniesSeenCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.Count, static count => new MarkFlyingDerbyBunniesSeenCommand(count)));
}
