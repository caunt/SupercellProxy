using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Releases;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>Reports the counters when a game mode was entered.</summary>
/// <param name="GameMode">The game mode.</param>
/// <param name="CounterIds">The counter ids.</param>
/// <param name="Counters">The counters.</param>
/// <param name="Timestamps">The timestamps.</param>
public sealed record ReportGameModeEnteredMessage(string? GameMode, ReadOnlyMemory<long> CounterIds, ReadOnlyMemory<int> Counters, ReadOnlyMemory<long> Timestamps)
    : CodecMessage<ReportGameModeEnteredMessage>, ICodecProvider<ReportGameModeEnteredMessage>
{
    /// <inheritdoc />
    public static ICodec<ReportGameModeEnteredMessage> Codec { get; } = Codecs.Versioned(
        Release17381.Version,
        Codecs.Composite(
            Codecs.OptionalString,
            static message => message.GameMode,
            Codecs.Memory(Codecs.VarLong),
            static message => message.CounterIds,
            Codecs.Memory(Codecs.VarInt),
            static message => message.Counters,
            Codecs.Memory(Codecs.VarLong),
            static message => message.Timestamps,
            static (gameMode, counterIds, counters, timestamps) => new ReportGameModeEnteredMessage(gameMode, counterIds, counters, timestamps)
        ),
        Codecs.Composite(
            Codecs.Memory(Codecs.VarLong),
            static message => message.Timestamps,
            Codecs.OptionalString,
            static message => message.GameMode,
            Codecs.Memory(Codecs.VarInt),
            static message => message.Counters,
            Codecs.Memory(Codecs.VarLong),
            static message => message.CounterIds,
            static (timestamps, gameMode, counters, counterIds) => new ReportGameModeEnteredMessage(gameMode, counterIds, counters, timestamps)
        )
    );
}
