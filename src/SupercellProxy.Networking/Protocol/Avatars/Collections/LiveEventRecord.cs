using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// Represents a seen or purchased live event the native <c language="csharp">LogicEventManager</c> remembers, saved with the keys
/// <c>id</c>, <c>st</c>, <c>cnt</c>, <c>imp</c> and <c>et</c>.
/// </summary>
/// <param name="EventId">The live event id.</param>
/// <param name="SchedulingType">1 when the event was targeted, which also records its impression id; otherwise 0.</param>
/// <param name="Count">How many times the event was seen or purchased.</param>
/// <param name="ImpressionId">The targeted event's impression id, present only when <paramref name="SchedulingType"/> is 1.</param>
/// <param name="EndTime">The event's end as a Unix timestamp.</param>
public sealed record LiveEventRecord(int EventId, int SchedulingType, int Count, int? ImpressionId, int EndTime) : ICodecProvider<LiveEventRecord>
{
    /// <inheritdoc />
    /// <remarks>Native writes the impression id only for scheduling type 1.</remarks>
    public static ICodec<LiveEventRecord> Codec { get; } = Codecs.Create(
        static stream =>
        {
            int eventId = stream.ReadVarInt();
            int schedulingType = stream.ReadVarInt();
            int count = stream.ReadVarInt();
            int? impressionId = schedulingType is 1 ? stream.ReadVarInt() : null;

            return new LiveEventRecord(eventId, schedulingType, count, impressionId, stream.ReadVarInt());
        },
        static (stream, record) =>
        {
            stream.WriteVarInt(record.EventId);
            stream.WriteVarInt(record.SchedulingType);
            stream.WriteVarInt(record.Count);

            if (record.SchedulingType is 1)
                stream.WriteVarInt(record.ImpressionId ?? throw new InvalidOperationException($"{nameof(ImpressionId)} is null."));

            stream.WriteVarInt(record.EndTime);
        }
    );
}
