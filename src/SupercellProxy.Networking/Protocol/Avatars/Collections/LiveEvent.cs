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
    public static ICodec<LiveEvent> Codec { get; } = Codecs.Composite(EventDocument.Codec,
        static liveEvent => new EventDocument(liveEvent.UsesCompressedData, liveEvent.Text, liveEvent.CompressedData),
        Codecs.VarInt,
        static liveEvent => liveEvent.EventId,
        Codecs.VarInt,
        static liveEvent => liveEvent.Version,
        Codecs.OptionalUtf8String,
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
        Codecs.OptionalUtf8String,
        static liveEvent => liveEvent.Category,
        static (document,
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
            category) => new LiveEvent
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
            });

    /// <summary>Gets or sets the event category, such as <c>ShopEvergreen</c> or <c>CSV_CHANGE</c>.</summary>
    /// <value>The optional string that ends the event's fields; <see langword="null"/> when the server sent no category.</value>
    public string? Category { get; init; }

    /// <summary>Gets or sets the zlib-compressed JSON definition when <see cref="UsesCompressedData"/> is set.</summary>
    /// <value>
    /// The raw compressed bytes exactly as received; <see langword="null"/> for a plain-text definition, which is in
    /// <see cref="Text"/> instead. <see cref="Decode{TValue}"/> inflates it on first use.
    /// </value>
    public ReadOnlyMemory<byte>? CompressedData { get; init; }

    /// <summary>Gets or sets the end of the event as a Unix timestamp.</summary>
    /// <value>
    /// Unix seconds, exclusive: the event runs while the server time is at least <see cref="StartTime"/> and before this value.
    /// </value>
    public int EndTime { get; init; }

    /// <summary>Gets or sets the event group id, or -1 without a group.</summary>
    /// <value>The variable-length integer read after <see cref="SchedulingType"/>; -1 when the event belongs to no group.</value>
    public int EventGroupId { get; init; }

    /// <summary>Gets or sets the live event id.</summary>
    /// <value>
    /// The id the event manager keys this event by in its flags, purchase and seen lists; shop purchase commands send it with
    /// <see cref="VariantId"/>.
    /// </value>
    public int EventId { get; init; }

    /// <summary>Gets or sets the top-level event group id, or -1 without a group.</summary>
    /// <value>
    /// The variable-length integer read after <see cref="EventGroupId"/>; -1 when the event belongs to no top-level group.
    /// </value>
    public int EventTopGroupId { get; init; }

    /// <summary>Gets or sets the impression id of a targeted event, or -1.</summary>
    /// <value>
    /// Identifies one showing of a targeted event (<see cref="SchedulingType"/> 1), so purchases and survey states are counted per
    /// showing; -1 for events that are not targeted.
    /// </value>
    public int ImpressionId { get; init; }

    /// <summary>Gets or sets the event name.</summary>
    /// <value>
    /// The optional internal name read after <see cref="Version"/>; <see langword="null"/> when not sent.
    /// </value>
    public string? Name { get; init; }

    /// <summary>
    /// Gets or sets how the server schedules the event; 1 marks a targeted event.
    /// </summary>
    /// <value>
    /// 1 for a targeted event, whose purchases are recorded per <see cref="ImpressionId"/> in the purchased events list; any other
    /// value records purchases by event id in the purchased shop ids list.
    /// </value>
    public int SchedulingType { get; init; }

    /// <summary>Gets or sets the start of the event as a Unix timestamp.</summary>
    /// <value>Unix seconds, inclusive: the first second at which the event is running.</value>
    public int StartTime { get; init; }

    /// <summary>Gets or sets the audience rule the server matched to give this player the event.</summary>
    /// <value>The rule's numeric id, read after <see cref="VisibleEndTime"/>; carried for re-encoding.</value>
    public int TargetingId { get; init; }

    /// <summary>Gets or sets the uncompressed JSON definition.</summary>
    /// <value>
    /// The definition text when <see cref="UsesCompressedData"/> is <see langword="false"/>; otherwise <see langword="null"/>.
    /// <see cref="Decode{TValue}"/> reads a missing text as an empty JSON object.
    /// </value>
    public string? Text { get; init; }

    /// <summary>Gets or sets the native live event type.</summary>
    /// <value>
    /// The type number that decides which definition the JSON holds; each definition type declares its own, for example 1 for shop
    /// offers and 8 for truck order events.
    /// </value>
    public int Type { get; init; }

    /// <summary>Gets or sets a value indicating whether the definition is sent compressed.</summary>
    /// <value>
    /// <see langword="true"/> if the leading flag says the definition follows as compressed bytes in <see cref="CompressedData"/>
    /// rather than as text in <see cref="Text"/>; otherwise, <see langword="false"/>.
    /// </value>
    public bool UsesCompressedData { get; init; }

    /// <summary>
    /// Gets or sets which version of the event this player got when the event exists in several variants; with the event id and
    /// version it identifies the exact definition.
    /// </summary>
    /// <value>
    /// The variant number read after <see cref="TargetingId"/>; shop purchase commands send it next to <see cref="EventId"/>.
    /// </value>
    public int VariantId { get; init; }

    /// <summary>Gets or sets the definition version.</summary>
    /// <value>
    /// The revision number read right after <see cref="EventId"/>; together with the event id and variant it tells changed
    /// definitions of the same event apart.
    /// </value>
    public int Version { get; init; }

    /// <summary>Gets or sets the end of the event's visibility as a Unix timestamp.</summary>
    /// <value>Unix seconds, exclusive: the event is shown before this second, which can differ from <see cref="EndTime"/>.</value>
    public int VisibleEndTime { get; init; }

    /// <summary>Gets or sets the start of the event's visibility as a Unix timestamp.</summary>
    /// <value>
    /// Unix seconds, inclusive: the event is shown from this second until <see cref="VisibleEndTime"/>, which can differ from its
    /// running window.
    /// </value>
    public int VisibleTime { get; init; }

    /// <summary>Deserializes the event's JSON definition; a compressed definition is inflated once per received event.</summary>
    /// <typeparam name="TValue">
    /// The definition record that matches <see cref="Type"/>, such as a truck order or shop offer definition; any reference type
    /// <see cref="JsonSerializer"/> can read.
    /// </typeparam>
    /// <returns>
    /// A new <typeparamref name="TValue"/> read from the definition; when neither text nor compressed bytes were sent, it is read
    /// from an empty JSON object.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// The definition JSON deserializes to <see langword="null"/> instead of a <typeparamref name="TValue"/> object.
    /// </exception>
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
        public static ICodec<EventDocument> Codec { get; } = Codecs.Dispatch(Codecs.Boolean,
            static document => document.UsesCompressedData,
            cases: [
                new KeyValuePair<bool, ICodec<EventDocument>>(key: true,
                    Codecs.Map(Codecs.OptionalByteArray,
                        static compressedData => new EventDocument(UsesCompressedData: true, Text: null, compressedData),
                        static document => document.CompressedData)),
                new KeyValuePair<bool, ICodec<EventDocument>>(key: false,
                    Codecs.Map(Codecs.OptionalUtf8String,
                        static text => new EventDocument(UsesCompressedData: false, text, CompressedData: null),
                        static document => document.Text)),
            ]);
    }
}
