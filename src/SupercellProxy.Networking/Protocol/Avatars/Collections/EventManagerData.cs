using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// Represents the avatar data of the native <c language="csharp">LogicEventManager</c>.
/// </summary>
public sealed record EventManagerData : ICodecProvider<EventManagerData>
{
    /// <inheritdoc />
    public static ICodec<EventManagerData> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static data => data.PreviousTime,
        Codecs.Optional(LiveEvents.Codec),
        static data => data.LiveEvents,
        Codecs.Array(Codecs.Pair(Codecs.VarInt, Codecs.Int32), CollectionCount.Bounded),
        static data => data.FixedValues,
        Codecs.Array(Codecs.Pair(Codecs.VarInt, Codecs.VarInt), CollectionCount.Bounded),
        static data => data.Pairs,
        Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
        static data => data.PurchasedShopIds,
        Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
        static data => data.SeenShopIds,
        Codecs.Array(Codecs.Pair(Codecs.VarInt, Codecs.OptionalString), CollectionCount.Bounded),
        static data => data.AvatarData,
        Codecs.Array(LiveEventRecord.Codec, CollectionCount.Bounded),
        static data => data.PurchasedEvents,
        Codecs.Array(LiveEventRecord.Codec, CollectionCount.Bounded),
        static data => data.SeenEvents,
        Codecs.Array(
            Codecs.Composite(
                Codecs.VarInt,
                static state => state.EventId,
                Codecs.VarInt,
                static state => state.ImpressionId,
                Codecs.VarInt,
                static state => state.State,
                static (eventId, impressionId, state) => new SurveyEventState(eventId, impressionId, state)
            ),
            CollectionCount.Bounded
        ),
        static data => data.SurveyEventStates,
        static (
            previousTime,
            liveEvents,
            fixedValues,
            pairs,
            purchasedShopIds,
            seenShopIds,
            avatarData,
            purchasedEvents,
            seenEvents,
            surveyEventStates
        ) => new EventManagerData
        {
            PreviousTime = previousTime,
            LiveEvents = liveEvents,
            FixedValues = fixedValues,
            Pairs = pairs,
            PurchasedShopIds = purchasedShopIds,
            SeenShopIds = seenShopIds,
            AvatarData = avatarData,
            PurchasedEvents = purchasedEvents,
            SeenEvents = seenEvents,
            SurveyEventStates = surveyEventStates,
        }
    );

    /// <summary>
    /// Gets the per-event flags keyed by live event id.
    /// </summary>
    public KeyValuePair<int, int>[] FixedValues { get; init; } = [];

    /// <summary>
    /// Gets the paired values native saves as <c>es_i</c> and <c>es_r</c>.
    /// </summary>
    public KeyValuePair<int, int>[] Pairs { get; init; } = [];

    /// <summary>
    /// Gets the scheduled live events.
    /// </summary>
    public LiveEvents? LiveEvents { get; init; }

    /// <summary>
    /// Gets the purchased shop offer ids, saved natively as <c>purchased_shop_ids</c>.
    /// </summary>
    public int[] PurchasedShopIds { get; init; } = [];

    /// <summary>
    /// Gets the previous update time, saved natively as <c>prev_time</c>.
    /// </summary>
    public int PreviousTime { get; init; }

    /// <summary>
    /// Gets the seen shop offer ids, saved natively as <c>seen_shop_ids</c>.
    /// </summary>
    public int[] SeenShopIds { get; init; } = [];

    /// <summary>
    /// Gets the per-event avatar JSON keyed by live event id, saved natively as <c>av_datas</c>.
    /// </summary>
    public KeyValuePair<int, string?>[] AvatarData { get; init; } = [];

    /// <summary>
    /// Gets the purchased events, saved natively as <c>purchased_events</c>.
    /// </summary>
    public LiveEventRecord[] PurchasedEvents { get; init; } = [];

    /// <summary>
    /// Gets the seen events, saved natively as <c>seen_events</c>.
    /// </summary>
    public LiveEventRecord[] SeenEvents { get; init; } = [];
    /// <summary>
    /// Gets the survey event states, saved natively as <c>survey_event_states_v2</c>.
    /// </summary>
    public SurveyEventState[] SurveyEventStates { get; init; } = [];
}
