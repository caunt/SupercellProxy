using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events;

/// <summary>
/// A native <c language="csharp">LogicLiveEvent</c> carried by a map-game event, encoded like <see cref="Avatars.Collections.LiveEvent"/> with required strings.
/// </summary>
public sealed record MapGameEventProfileData : ICodecProvider<MapGameEventProfileData>
{
    /// <summary>Initializes a new <see cref="MapGameEventProfileData"/> instance.</summary>
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
    public static ICodec<MapGameEventProfileData> Codec { get; } = Codecs.Composite(
        ProfileDocument.Codec,
        static profile => new ProfileDocument(profile.UsesBinaryData, profile.BinaryData, profile.OptionalTextData),
        Codecs.VarInt,
        static profile => profile.EventId,
        Codecs.VarInt,
        static profile => profile.Version,
        Codecs.String,
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
        Codecs.String,
        static profile => profile.Category,
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
        ) => new MapGameEventProfileData(document.UsesBinaryData, document.BinaryData, document.OptionalTextData)
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
        }
    );

    /// <summary>Gets the zlib-compressed JSON definition when <see cref="UsesBinaryData"/> is set.</summary>
    public ReadOnlyMemory<byte>? BinaryData { get; }

    /// <summary>Gets the event category.</summary>
    public string Category { get; init; } = string.Empty;

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
    public string Name { get; init; } = string.Empty;

    /// <summary>Gets the uncompressed JSON definition.</summary>
    public string? OptionalTextData { get; }

    /// <summary>Gets the scheduling type.</summary>
    public int SchedulingType { get; init; }

    /// <summary>Gets the start of the event as a Unix timestamp.</summary>
    public int StartTime { get; init; }

    /// <summary>Gets the targeting id.</summary>
    public int TargetingId { get; init; }

    /// <summary>Gets the native live event type.</summary>
    public int Type { get; init; }

    /// <summary>Gets a value indicating whether the definition is sent compressed.</summary>
    public bool UsesBinaryData { get; }

    /// <summary>Gets the variant id.</summary>
    public int VariantId { get; init; }

    /// <summary>Gets the definition version.</summary>
    public int Version { get; init; }

    /// <summary>Gets the end of the event's visibility as a Unix timestamp.</summary>
    public int VisibleEndTime { get; init; }

    /// <summary>Gets the start of the event's visibility as a Unix timestamp.</summary>
    public int VisibleTime { get; init; }

    // Native sends the profile document either as binary data or as optional text, chosen by the leading flag.
    private readonly record struct ProfileDocument(bool UsesBinaryData, ReadOnlyMemory<byte>? BinaryData, string? OptionalTextData)
    {
        public static ICodec<ProfileDocument> Codec { get; } = Codecs.Dispatch(
            Codecs.Boolean,
            static document => document.UsesBinaryData,
            [
                new KeyValuePair<bool, ICodec<ProfileDocument>>(
                    key: true,
                    Codecs.Map(
                        Codecs.OptionalByteArray,
                        static binaryData => new ProfileDocument(UsesBinaryData: true, binaryData, OptionalTextData: null),
                        static document => document.BinaryData
                    )
                ),
                new KeyValuePair<bool, ICodec<ProfileDocument>>(
                    key: false,
                    Codecs.Map(
                        Codecs.OptionalString,
                        static optionalTextData => new ProfileDocument(UsesBinaryData: false, BinaryData: null, optionalTextData),
                        static document => document.OptionalTextData
                    )
                ),
            ]
        );
    }
}
