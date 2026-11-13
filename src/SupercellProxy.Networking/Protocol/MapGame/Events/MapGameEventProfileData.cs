using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events;

/// <summary>
/// A native <c language="csharp">LogicLiveEvent</c> carried by a map-game event, encoded like <see cref="Avatars.Collections.LiveEvent"/> with required strings.
/// </summary>
public sealed record MapGameEventProfileData : ICodecProvider<MapGameEventProfileData>
{
    /// <summary>Initializes a new instance of the <see cref="MapGameEventProfileData"/> record. Holds the event definition in exactly one of its two forms: compressed bytes or plain text.</summary>
    /// <param name="usesBinaryData">True when the definition is carried as compressed bytes.</param>
    /// <param name="binaryData">The compressed definition; must be <see langword="null"/> for the text form.</param>
    /// <param name="optionalTextData">The plain-text definition; must be <see langword="null"/> for the compressed form.</param>
    /// <exception cref="InvalidDataException">Data of the other form was also given.</exception>
    public MapGameEventProfileData(bool usesBinaryData, ReadOnlyMemory<byte>? binaryData, string? optionalTextData)
    {
        if (usesBinaryData && optionalTextData is not null)
            throw new InvalidDataException(message: "A binary map-game event profile cannot contain optional text data.");

        if (!usesBinaryData && binaryData is not null)
            throw new InvalidDataException(message: "A text map-game event profile cannot contain binary data.");

        UsesBinaryData = usesBinaryData;
        BinaryData = binaryData is null ? null : (ReadOnlyMemory<byte>?)binaryData.Value.ToArray();
        OptionalTextData = optionalTextData;
    }

    /// <inheritdoc />
    public static ICodec<MapGameEventProfileData> Codec { get; } = Codecs.Composite(ProfileDocument.Codec,
        static profile => new ProfileDocument(profile.UsesBinaryData, profile.BinaryData, profile.OptionalTextData),
        Codecs.VarInt,
        static profile => profile.EventId,
        Codecs.VarInt,
        static profile => profile.Version,
        Codecs.Utf8String,
        static profile => profile.Name,
        Codecs.VarInt,
        static profile => profile.Type,
        Codecs.VarInt,
        static profile => profile.StartTime,
        Codecs.VarInt,
        static profile => profile.EndTime,
        Codecs.VarInt,
        static profile => profile.VisibleTime,
        Codecs.VarInt,
        static profile => profile.VisibleEndTime,
        Codecs.VarInt,
        static profile => profile.TargetingId,
        Codecs.VarInt,
        static profile => profile.VariantId,
        Codecs.VarInt,
        static profile => profile.SchedulingType,
        Codecs.VarInt,
        static profile => profile.EventGroupId,
        Codecs.VarInt,
        static profile => profile.EventTopGroupId,
        Codecs.VarInt,
        static profile => profile.ImpressionId,
        Codecs.Utf8String,
        static profile => profile.Category,
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
            category) => new MapGameEventProfileData(document.UsesBinaryData, document.BinaryData, document.OptionalTextData)
            {
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

    /// <summary>Gets the zlib-compressed JSON definition when <see cref="UsesBinaryData"/> is set.</summary>
    /// <value>A private copy of the compressed bytes passed to the constructor; <see langword="null"/> for the text form, or when the binary form carried no bytes.</value>
    public ReadOnlyMemory<byte>? BinaryData { get; }

    /// <summary>Gets or sets the event category.</summary>
    /// <value>The required string that closes the profile on the wire; empty by default.</value>
    public string Category { get; init; } = string.Empty;

    /// <summary>Gets or sets the end of the event as a Unix timestamp.</summary>
    /// <value>Unix seconds at which the event stops running, read after <see cref="StartTime"/>.</value>
    public int EndTime { get; init; }

    /// <summary>Gets or sets the event group id, or -1 without a group.</summary>
    /// <value>The group number read after <see cref="SchedulingType"/>; -1 when the event is not grouped.</value>
    public int EventGroupId { get; init; }

    /// <summary>Gets or sets the live event id.</summary>
    /// <value>The event's numeric id, the first field after the definition document; other live-event data refers to the event by it.</value>
    public int EventId { get; init; }

    /// <summary>Gets or sets the top-level event group id, or -1 without a group.</summary>
    /// <value>The outer group number read after <see cref="EventGroupId"/>; -1 when the event has no top-level group.</value>
    public int EventTopGroupId { get; init; }

    /// <summary>Gets or sets the impression id of a targeted event, or -1.</summary>
    /// <value>The id of this showing of a targeted event (<see cref="SchedulingType"/> 1), read after <see cref="EventTopGroupId"/>; -1 for an untargeted event.</value>
    public int ImpressionId { get; init; }

    /// <summary>Gets or sets the event name.</summary>
    /// <value>The event's internal name, a required string read after <see cref="Version"/>; empty by default.</value>
    public string Name { get; init; } = string.Empty;

    /// <summary>Gets the uncompressed JSON definition.</summary>
    /// <value>The definition text when <see cref="UsesBinaryData"/> is <see langword="false"/>; <see langword="null"/> for the compressed form or when no text was sent.</value>
    public string? OptionalTextData { get; }

    /// <summary>Gets or sets how the server schedules the event; 1 marks a targeted event.</summary>
    /// <value>The scheduling kind read after <see cref="VariantId"/>; 1 ties the event to <see cref="ImpressionId"/>, and other values are not told apart here.</value>
    public int SchedulingType { get; init; }

    /// <summary>Gets or sets the start of the event as a Unix timestamp.</summary>
    /// <value>Unix seconds from which the event runs, read after <see cref="Type"/>.</value>
    public int StartTime { get; init; }

    /// <summary>Gets or sets the audience rule the server matched to give this player the event.</summary>
    /// <value>The rule's numeric id, read after <see cref="VisibleEndTime"/> and kept for re-encoding.</value>
    public int TargetingId { get; init; }

    /// <summary>Gets or sets the native live event type.</summary>
    /// <value>The type number that tells which definition the JSON holds, read after <see cref="Name"/>.</value>
    public int Type { get; init; }

    /// <summary>Gets a value indicating whether the definition is sent compressed.</summary>
    /// <value><see langword="true"/> if the leading wire flag selects the compressed form in <see cref="BinaryData"/>; otherwise, <see langword="false"/>.</value>
    /// <remarks>When the flag is <see langword="false"/>, the definition is in <see cref="OptionalTextData"/>.</remarks>
    public bool UsesBinaryData { get; }

    /// <summary>Gets or sets which version of the event this player got when the event exists in several variants.</summary>
    /// <value>The variant number read after <see cref="TargetingId"/>; with <see cref="EventId"/> and <see cref="Version"/> it names the exact definition this player received.</value>
    public int VariantId { get; init; }

    /// <summary>Gets or sets the definition version.</summary>
    /// <value>The revision number read right after <see cref="EventId"/>; a changed definition of the same event carries a different version.</value>
    public int Version { get; init; }

    /// <summary>Gets or sets the end of the event's visibility as a Unix timestamp.</summary>
    /// <value>Unix seconds after which the event is no longer shown, read after <see cref="VisibleTime"/>; it can differ from <see cref="EndTime"/>.</value>
    public int VisibleEndTime { get; init; }

    /// <summary>Gets or sets the start of the event's visibility as a Unix timestamp.</summary>
    /// <value>Unix seconds from which the event is shown, read after <see cref="EndTime"/>; it can differ from <see cref="StartTime"/>.</value>
    public int VisibleTime { get; init; }

    // Native sends the profile document either as binary data or as optional text, chosen by the leading flag.
    private readonly record struct ProfileDocument(bool UsesBinaryData, ReadOnlyMemory<byte>? BinaryData, string? OptionalTextData)
    {
        public static ICodec<ProfileDocument> Codec { get; } = Codecs.Dispatch(Codecs.Boolean,
            static document => document.UsesBinaryData,
            cases:
            [
                new KeyValuePair<bool, ICodec<ProfileDocument>>(key: true,
                    Codecs.Map(Codecs.OptionalByteArray,
                        static binaryData => new ProfileDocument(UsesBinaryData: true, binaryData, OptionalTextData: null),
                        static document => document.BinaryData)),
                new KeyValuePair<bool, ICodec<ProfileDocument>>(key: false,
                    Codecs.Map(Codecs.OptionalUtf8String,
                        static optionalTextData => new ProfileDocument(UsesBinaryData: false, BinaryData: null, optionalTextData),
                        static document => document.OptionalTextData)),
            ]);
    }
}
