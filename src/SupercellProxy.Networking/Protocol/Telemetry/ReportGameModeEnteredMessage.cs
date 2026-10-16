using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>
/// Reports numbered steps the client went through, such as the start-up steps 0 to 3 and then 8, 11 and 12 in the captures, each
/// with two times.
/// </summary>
/// <param name="GameMode">Text sent with the steps; it holds the device model, such as SM-S908B, in every capture.</param>
/// <param name="CounterIds">Second Unix time in seconds of each step, never earlier than the matching first time.</param>
/// <param name="Counters">Number of each reported step.</param>
/// <param name="Timestamps">First Unix time in seconds of each step.</param>
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
