using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Seen;

/// <summary>Marks the Valley sun points as seen.</summary>
public sealed record MarkMapGameSunPointsSeenCommand : CodecCommand<MarkMapGameSunPointsSeenCommand>, ICommandCodecProvider<MarkMapGameSunPointsSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkMapGameSunPointsSeenCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new MarkMapGameSunPointsSeenCommand()));
}
