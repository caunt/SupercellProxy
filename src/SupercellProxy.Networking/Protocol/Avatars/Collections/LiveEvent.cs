using System.Runtime.CompilerServices;
using System.Text.Json;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// Represents a native <c language="csharp">LogicLiveEvent</c>: one scheduled live event with its JSON definition.
/// </summary>
public sealed record LiveEvent : ICodecProvider<LiveEvent>
{
    // A received event's definition never changes, while the game reads it every tick, so its inflated JSON is kept with the instance.
    private static readonly ConditionalWeakTable<LiveEvent, byte[]> InflatedDocuments = [];

    /// <inheritdoc />
    public static ICodec<LiveEvent> Codec { get; } = Codecs.Composite(
        EventDocument.Codec,
        static liveEvent => new EventDocument(liveEvent.UsesCompressedData, liveEvent.Text, liveEvent.CompressedData),
        Codecs.VarInt,
        static liveEvent => liveEvent.EventId,
        Codecs.VarInt,
        static liveEvent => liveEvent.Version,
        Codecs.OptionalString,
        static liveEvent => liveEvent.Name,
        Codecs.VarInt,
        static liveEvent => liveEvent.Type,
        Codecs.VarInt,
        static liveEvent => liveEvent.StartTime,
        Codecs.VarInt,
        static liveEvent => liveEvent.EndTime,
        Codecs.VarInt,
        static liveEvent => liveEvent.VisibleTime,
        Codecs.VarInt,
        static liveEvent => liveEvent.VisibleEndTime,
        Codecs.VarInt,
        static liveEvent => liveEvent.TargetingId,
        Codecs.VarInt,
        static liveEvent => liveEvent.VariantId,
        Codecs.VarInt,
        static liveEvent => liveEvent.SchedulingType,
        Codecs.VarInt,
        static liveEvent => liveEvent.EventGroupId,
        Codecs.VarInt,
        static liveEvent => liveEvent.EventTopGroupId,
        Codecs.VarInt,
        static liveEvent => liveEvent.ImpressionId,
        Codecs.OptionalString,
        static liveEvent => liveEvent.Category,
        static (
            document,
            eventId,
            version,
            name,
            type,
            startTime,
            endTime,
            visibleTime,
            visibleEndTime,
            targetingId,
            variantId,
            schedulingType,
            eventGroupId,
            eventTopGroupId,
            impressionId,
            category
        ) => new LiveEvent
        {
            UsesCompressedData = document.UsesCompressedData,
            Text = document.Text,
            CompressedData = document.CompressedData,
            EventId = eventId,
            Version = version,
            Name = name,
            Type = type,
            StartTime = startTime,
            EndTime = endTime,
            VisibleTime = visibleTime,
            VisibleEndTime = visibleEndTime,
            TargetingId = targetingId,
            VariantId = variantId,
            SchedulingType = schedulingType,
            EventGroupId = eventGroupId,
            EventTopGroupId = eventTopGroupId,
            ImpressionId = impressionId,
            Category = category,
        }
    );

    /// <summary>Gets the event category, such as <c>ShopEvergreen</c> or <c>CSV_CHANGE</c>.</summary>
    public string? Category { get; init; }

    /// <summary>Gets the zlib-compressed JSON definition when <see cref="UsesCompressedData"/> is set.</summary>
    public ReadOnlyMemory<byte>? CompressedData { get; init; }

    /// <summary>Gets the end of the event as a Unix timestamp.</summary>
    public int EndTime { get; init; }

    /// <summary>Gets the event group id, or -1 without a group.</summary>
    public int EventGroupId { get; init; }

    /// <summary>Gets the live event id.</summary>
    public int EventId { get; init; }

    /// <summary>Gets the top-level event group id, or -1 without a group.</summary>
    public int EventTopGroupId { get; init; }

    /// <summary>Gets the impression id of a targeted event, or -1.</summary>
    public int ImpressionId { get; init; }

    /// <summary>Gets the event name.</summary>
    public string? Name { get; init; }

    /// <summary>How the server schedules the event; 1 marks a targeted event that is remembered per showing once seen.</summary>
    public int SchedulingType { get; init; }

    /// <summary>Gets the start of the event as a Unix timestamp.</summary>
    public int StartTime { get; init; }

    /// <summary>Audience rule the server matched to give this player the event.</summary>
    public int TargetingId { get; init; }

    /// <summary>Gets the uncompressed JSON definition.</summary>
    public string? Text { get; init; }

    /// <summary>Gets the native live event type.</summary>
    public int Type { get; init; }

    /// <summary>Gets a value indicating whether the definition is sent compressed.</summary>
    public bool UsesCompressedData { get; init; }

    /// <summary>
    /// Which version of the event this player got when the event exists in several variants; with the event id and version it
    /// identifies the exact definition.
    /// </summary>
    public int VariantId { get; init; }

    /// <summary>Gets the definition version.</summary>
    public int Version { get; init; }

    /// <summary>Gets the end of the event's visibility as a Unix timestamp.</summary>
    public int VisibleEndTime { get; init; }

    /// <summary>Gets the start of the event's visibility as a Unix timestamp.</summary>
    public int VisibleTime { get; init; }

    /// <summary>Deserializes the event's JSON definition; a compressed definition is inflated once per received event.</summary>
    public TValue Decode<TValue>() where TValue : class
    {
        if (CompressedData is null)
        {
            return JsonSerializer.Deserialize<TValue>(Text ?? "{}")
                ?? throw new InvalidDataException(message: "The event document is null.");
        }

        byte[] document = InflatedDocuments.GetValue(this, static liveEvent => liveEvent.CompressedData is { } bytes ? CompressedDocument.Decompress(bytes) : []);

        return JsonSerializer.Deserialize<TValue>(document)
            ?? throw new InvalidDataException($"Compressed JSON did not contain a {typeof(TValue).Name} value.");
    }

    // Native sends the event definition either as compressed JSON or as plain text, chosen by the leading flag.
    private readonly record struct EventDocument(bool UsesCompressedData, string? Text, ReadOnlyMemory<byte>? CompressedData)
    {
        public static ICodec<EventDocument> Codec { get; } = Codecs.Dispatch(
            Codecs.Boolean,
            static document => document.UsesCompressedData,
            [
                new KeyValuePair<bool, ICodec<EventDocument>>(
                    key: true,
                    Codecs.Map(
                        Codecs.OptionalByteArray,
                        static compressedData => new EventDocument(UsesCompressedData: true, Text: null, compressedData),
                        static document => document.CompressedData
                    )
                ),
                new KeyValuePair<bool, ICodec<EventDocument>>(
                    key: false,
                    Codecs.Map(
                        Codecs.OptionalString,
                        static text => new EventDocument(UsesCompressedData: false, text, CompressedData: null),
                        static document => document.Text
                    )
                ),
            ]
        );
    }
}
