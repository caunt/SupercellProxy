using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.ShopEvents.Offers;

namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>
/// <para>One shop event in server command 355.</para>
/// </summary>
public sealed record ShopEvent : ICodecProvider<ShopEvent>
{
    /// <inheritdoc />
    public static ICodec<ShopEvent> Codec { get; } = Codecs.Composite(EventDocument.Codec,
        static shopEvent => new EventDocument(shopEvent.BinaryData, shopEvent.TextData),
        Codecs.VarInt,
        static shopEvent => shopEvent.EventId,
        Codecs.VarInt,
        static shopEvent => shopEvent.Version,
        Codecs.Utf8String,
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
        Codecs.Utf8String,
        static shopEvent => shopEvent.Category,
        static (eventDocument,
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
            category) => new ShopEvent
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
            });

    /// <summary>
    /// Gets or sets the event's JSON definition in compressed form, when the server sends it that way.
    /// </summary>
    /// <value>The length-prefixed bytes that follow a leading <see langword="true"/> flag on the wire; <see langword="null"/> when the definition came as <see cref="TextData"/> instead.</value>
    public ReadOnlyMemory<byte>? BinaryData { get; init; }

    /// <summary>
    /// Gets or sets the event's JSON definition as plain text, when the server sends it that way; empty otherwise.
    /// </summary>
    /// <value>The string that follows a leading <see langword="false"/> flag on the wire; it is written only while <see cref="BinaryData"/> is <see langword="null"/>.</value>
    public string TextData { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the end of the event as a Unix timestamp.
    /// </summary>
    /// <value>Seconds since the Unix epoch, sent as a var-int after the start time; the event stops being active at this moment.</value>
    public int EndTime { get; init; }

    /// <summary>
    /// Gets or sets the event group id, or -1 without a group.
    /// </summary>
    /// <value>A var-int sent after the scheduling type that groups related events; -1 when the event belongs to no group.</value>
    public int EventGroupId { get; init; }

    /// <summary>
    /// Gets or sets the server number of the shop event, which purchases and seen records refer to.
    /// </summary>
    /// <value>The var-int id sent right after the definition and serialized to JSON as <c>EventId</c>; shop purchase commands name the event by it.</value>
    [JsonPropertyName("EventId")]
    public int EventId { get; init; }

    /// <summary>
    /// Gets or sets the top-level event group id, or -1 without a group.
    /// </summary>
    /// <value>A var-int sent after <see cref="EventGroupId"/> naming the outermost group the event sits in; -1 when there is none.</value>
    public int EventTopGroupId { get; init; }

    /// <summary>
    /// Gets or sets the native live-event type number, which decides how the definition is read and what the event does.
    /// </summary>
    /// <value>A var-int sent after the name; for example <see cref="ShopOfferDefinition.EventType"/> (1) marks a shop offer whose definition is a <see cref="ShopOfferDefinition"/>.</value>
    public int EventType { get; init; }

    /// <summary>
    /// Gets or sets the impression id of a targeted event, or -1.
    /// </summary>
    /// <value>A var-int sent just before the category; seen records store it so a targeted event is remembered per showing.</value>
    public int ImpressionId { get; init; }

    /// <summary>
    /// Gets or sets the event name.
    /// </summary>
    /// <value>The internal name string sent after the version; empty by default.</value>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets how the server schedules the event; 1 marks a targeted event that is remembered per showing once seen.
    /// </summary>
    /// <value>A var-int sent after the variant id; 0 for an ordinary scheduled event and 1 for a targeted one.</value>
    public int SchedulingType { get; init; }

    /// <summary>
    /// Gets or sets the start of the event as a Unix timestamp.
    /// </summary>
    /// <value>Seconds since the Unix epoch, sent as a var-int after the event type; the event becomes active at this moment.</value>
    public int StartTime { get; init; }

    /// <summary>
    /// Gets or sets the audience rule the server matched to give this player the event.
    /// </summary>
    /// <value>A server-side id sent as a var-int after the visibility window; the rule it names is not described in the payload.</value>
    public int TargetingId { get; init; }

    /// <summary>
    /// Gets or sets the event category.
    /// </summary>
    /// <value>The last string of the event on the wire, such as <c>ShopEvergreen</c> or <c>CSV_CHANGE</c>; empty by default.</value>
    public string Category { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets which version of the event this player got when the event exists in several variants.
    /// </summary>
    /// <value>A var-int sent after the targeting id; shop purchase commands send it back together with <see cref="EventId"/>.</value>
    public int VariantId { get; init; }

    /// <summary>
    /// Gets or sets the definition version.
    /// </summary>
    /// <value>A var-int sent right after <see cref="EventId"/> that numbers the revision of this event's definition.</value>
    public int Version { get; init; }

    /// <summary>
    /// Gets or sets the end of the event's visibility as a Unix timestamp.
    /// </summary>
    /// <value>Seconds since the Unix epoch, sent as a var-int after <see cref="VisibleTime"/>; after this the player no longer sees the event.</value>
    public int VisibleEndTime { get; init; }

    /// <summary>
    /// Gets or sets the start of the event's visibility as a Unix timestamp.
    /// </summary>
    /// <value>Seconds since the Unix epoch, sent as a var-int after <see cref="EndTime"/>; the player can see the event from this moment on.</value>
    public int VisibleTime { get; init; }

    // Native sends the event definition either as binary data or as text, chosen by the leading flag.
    private readonly record struct EventDocument(ReadOnlyMemory<byte>? BinaryData, string TextData)
    {
        public static ICodec<EventDocument> Codec { get; } = Codecs.Dispatch(Codecs.Boolean,
            static document => document.BinaryData is not null,
            cases: [
                new KeyValuePair<bool, ICodec<EventDocument>>(key: true,
                    Codecs.Map(Codecs.ByteArray,
                        static binaryData => new EventDocument(binaryData, string.Empty),
                        static document => document.BinaryData.GetValueOrDefault())),
                new KeyValuePair<bool, ICodec<EventDocument>>(key: false,
                    Codecs.Map(Codecs.Utf8String, static textData => new EventDocument(BinaryData: null, textData), static document => document.TextData)),
            ]);
    }
}
