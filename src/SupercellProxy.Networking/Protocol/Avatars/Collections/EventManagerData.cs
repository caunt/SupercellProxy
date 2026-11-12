using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// Represents the avatar data of the native <c language="csharp">LogicEventManager</c>.
/// </summary>
public sealed record EventManagerData : ICodecProvider<EventManagerData>
{
    /// <inheritdoc />
    public static ICodec<EventManagerData> Codec { get; } = Codecs.Composite(Codecs.VarInt,
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
        Codecs.Array(Codecs.Composite(Codecs.VarInt,
                static state => state.EventId,
                Codecs.VarInt,
                static state => state.ImpressionId,
                Codecs.VarInt,
                static state => state.State,
                static (eventId, impressionId, state) => new SurveyEventState(eventId, impressionId, state)),
            CollectionCount.Bounded),
        static data => data.SurveyEventStates,
        static (previousTime,
            liveEvents,
            fixedValues,
            pairs,
            purchasedShopIds,
            seenShopIds,
            avatarData,
            purchasedEvents,
            seenEvents,
            surveyEventStates) => new EventManagerData
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
            });

    /// <summary>
    /// Gets or sets the per-event flags keyed by live event id.
    /// </summary>
    /// <value>
    /// Pairs of a live event id and a fixed-width 32-bit flag, read after <see cref="LiveEvents"/>; empty by default.
    /// </value>
    public KeyValuePair<int, int>[] FixedValues { get; init; } = [];

    /// <summary>
    /// Gets or sets the paired values native saves as <c>es_i</c> and <c>es_r</c>.
    /// </summary>
    /// <value>
    /// Pairs of variable-length integers read after <see cref="FixedValues"/>; what the two numbers mean beyond those save keys is
    /// not identified. Empty by default.
    /// </value>
    public KeyValuePair<int, int>[] Pairs { get; init; } = [];

    /// <summary>
    /// Gets or sets the scheduled live events.
    /// </summary>
    /// <value>
    /// The optional event list read after <see cref="PreviousTime"/>; <see langword="null"/> when the server sent none.
    /// </value>
    public LiveEvents? LiveEvents { get; init; }

    /// <summary>
    /// Gets or sets the purchased shop offer ids, saved natively as <c>purchased_shop_ids</c>.
    /// </summary>
    /// <value>
    /// One live event id per purchase from an offer that is not targeted (scheduling type other than 1), so an id repeats once for
    /// each time the offer was bought. Empty by default.
    /// </value>
    public int[] PurchasedShopIds { get; init; } = [];

    /// <summary>
    /// Gets or sets the previous update time, saved natively as <c>prev_time</c>.
    /// </summary>
    /// <value>
    /// The first variable-length integer of the block; its unit is not identified, and it is carried unchanged for re-encoding.
    /// </value>
    public int PreviousTime { get; init; }

    /// <summary>
    /// Gets or sets the seen shop offer ids, saved natively as <c>seen_shop_ids</c>.
    /// </summary>
    /// <value>
    /// Live event ids of shop offers the player has seen, read after <see cref="PurchasedShopIds"/>; carried for re-encoding. Empty by
    /// default.
    /// </value>
    public int[] SeenShopIds { get; init; } = [];

    /// <summary>
    /// Gets or sets the per-event avatar JSON keyed by live event id, saved natively as <c>av_datas</c>.
    /// </summary>
    /// <value>
    /// Pairs of a live event id and that event's optional JSON state for this player, read after <see cref="SeenShopIds"/>; the text
    /// is <see langword="null"/> when the entry was sent without one.
    /// </value>
    public KeyValuePair<int, string?>[] AvatarData { get; init; } = [];

    /// <summary>
    /// Gets or sets the purchased events, saved natively as <c>purchased_events</c>.
    /// </summary>
    /// <value>
    /// One record per showing of a targeted (scheduling type 1) event the player bought from, identified by event id and impression
    /// id, with <see cref="LiveEventRecord.Count"/> as the number of purchases in that showing. Empty by default.
    /// </value>
    public LiveEventRecord[] PurchasedEvents { get; init; } = [];

    /// <summary>
    /// Gets or sets the seen events, saved natively as <c>seen_events</c>.
    /// </summary>
    /// <value>
    /// Records in the layout of <see cref="PurchasedEvents"/>, read after it, whose <see cref="LiveEventRecord.Count"/> says how often
    /// each event was seen; carried for re-encoding.
    /// </value>
    public LiveEventRecord[] SeenEvents { get; init; } = [];
    /// <summary>
    /// Gets or sets the survey event states, saved natively as <c>survey_event_states_v2</c>.
    /// </summary>
    /// <value>
    /// One entry per survey showing with its event id, impression id and progress state; the last list of the block, empty by
    /// default.
    /// </value>
    public SurveyEventState[] SurveyEventStates { get; init; } = [];
}
