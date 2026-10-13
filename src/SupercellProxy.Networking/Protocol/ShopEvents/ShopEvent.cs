using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>
/// <para>One shop event in server command 355.</para>
/// </summary>
public sealed record ShopEvent : ICodecProvider<ShopEvent>
{
    /// <inheritdoc />
    public static ICodec<ShopEvent> Codec { get; } = Codecs.Composite(
        EventDocument.Codec,
        static shopEvent => new EventDocument(shopEvent.BinaryData, shopEvent.TextData),
        Codecs.VarInt,
        static shopEvent => shopEvent.EventId,
        Codecs.VarInt,
        static shopEvent => shopEvent.Version,
        Codecs.String,
        static shopEvent => shopEvent.Name,
        Codecs.VarInt,
        static shopEvent => shopEvent.EventType,
        Codecs.VarInt,
        static shopEvent => shopEvent.StartTime,
        Codecs.VarInt,
        static shopEvent => shopEvent.EndTime,
        Codecs.VarInt,
        static shopEvent => shopEvent.VisibleTime,
        Codecs.VarInt,
        static shopEvent => shopEvent.VisibleEndTime,
        Codecs.VarInt,
        static shopEvent => shopEvent.TargetingId,
        Codecs.VarInt,
        static shopEvent => shopEvent.VariantId,
        Codecs.VarInt,
        static shopEvent => shopEvent.SchedulingType,
        Codecs.VarInt,
        static shopEvent => shopEvent.EventGroupId,
        Codecs.VarInt,
        static shopEvent => shopEvent.EventTopGroupId,
        Codecs.VarInt,
        static shopEvent => shopEvent.ImpressionId,
        Codecs.String,
        static shopEvent => shopEvent.Category,
        static (
            eventDocument,
            eventId,
            version,
            name,
            eventType,
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
        ) => new ShopEvent
        {
            BinaryData = eventDocument.BinaryData,
            TextData = eventDocument.TextData,
            EventId = eventId,
            Version = version,
            Name = name,
            EventType = eventType,
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

    /// <summary>
    /// Gets or sets the <c language="csharp">BinaryData</c> value.
    /// </summary>
    public ReadOnlyMemory<byte>? BinaryData { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TextData</c> value.
    /// </summary>
    public string TextData { get; init; } = string.Empty;

    /// <summary>
    /// Gets the end of the event as a Unix timestamp.
    /// </summary>
    public int EndTime { get; init; }

    /// <summary>
    /// Gets the event group id, or -1 without a group.
    /// </summary>
    public int EventGroupId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">EventId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("EventId")]
    public int EventId { get; init; }

    /// <summary>
    /// Gets the top-level event group id, or -1 without a group.
    /// </summary>
    public int EventTopGroupId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">EventType</c> value.
    /// </summary>
    public int EventType { get; init; }

    /// <summary>
    /// Gets the impression id of a targeted event, or -1.
    /// </summary>
    public int ImpressionId { get; init; }

    /// <summary>
    /// Gets the event name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the scheduling type.
    /// </summary>
    public int SchedulingType { get; init; }

    /// <summary>
    /// Gets the start of the event as a Unix timestamp.
    /// </summary>
    public int StartTime { get; init; }

    /// <summary>
    /// Gets the targeting id.
    /// </summary>
    public int TargetingId { get; init; }

    /// <summary>
    /// Gets the event category.
    /// </summary>
    public string Category { get; init; } = string.Empty;

    /// <summary>
    /// Gets the variant id.
    /// </summary>
    public int VariantId { get; init; }

    /// <summary>
    /// Gets the definition version.
    /// </summary>
    public int Version { get; init; }

    /// <summary>
    /// Gets the end of the event's visibility as a Unix timestamp.
    /// </summary>
    public int VisibleEndTime { get; init; }

    /// <summary>
    /// Gets the start of the event's visibility as a Unix timestamp.
    /// </summary>
    public int VisibleTime { get; init; }

    // Native sends the event definition either as binary data or as text, chosen by the leading flag.
    private readonly record struct EventDocument(ReadOnlyMemory<byte>? BinaryData, string TextData)
    {
        public static ICodec<EventDocument> Codec { get; } = Codecs.Dispatch(
            Codecs.Boolean,
            static document => document.BinaryData is not null,
            [
                new KeyValuePair<bool, ICodec<EventDocument>>(
                    key: true,
                    Codecs.Map(
                        Codecs.ByteArray,
                        static binaryData => new EventDocument(binaryData, string.Empty),
                        static document => document.BinaryData.GetValueOrDefault()
                    )
                ),
                new KeyValuePair<bool, ICodec<EventDocument>>(
                    key: false,
                    Codecs.Map(Codecs.String, static textData => new EventDocument(BinaryData: null, textData), static document => document.TextData)
                ),
            ]
        );
    }
}
