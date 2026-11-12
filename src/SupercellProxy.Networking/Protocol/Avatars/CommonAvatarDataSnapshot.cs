using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Accounts.Rewards;

using SupercellProxy.Networking.Protocol.Boats.Snapshots;
using SupercellProxy.Networking.Protocol.Boosters;
using SupercellProxy.Networking.Protocol.Creatures;
using SupercellProxy.Networking.Protocol.Customization;
using SupercellProxy.Networking.Protocol.Emotes;
using SupercellProxy.Networking.Protocol.Events.Calendar;
using SupercellProxy.Networking.Protocol.Events.ChainOffers;
using SupercellProxy.Networking.Protocol.Events.Chronos;
using SupercellProxy.Networking.Protocol.Events.Decoration;
using SupercellProxy.Networking.Protocol.Events.Decoration.StickerBook;
using SupercellProxy.Networking.Protocol.Events.Derby;
using SupercellProxy.Networking.Protocol.FarmPass;
using SupercellProxy.Networking.Protocol.Gifts;
using SupercellProxy.Networking.Protocol.MapGame;
using SupercellProxy.Networking.Protocol.MiniPass;
using SupercellProxy.Networking.Protocol.MovieTickets;
using SupercellProxy.Networking.Protocol.Neighborhoods;
using SupercellProxy.Networking.Protocol.Neighborhoods.Requests;
using SupercellProxy.Networking.Protocol.Newspapers;
using SupercellProxy.Networking.Protocol.Orders;
using SupercellProxy.Networking.Protocol.Production;
using SupercellProxy.Networking.Protocol.Reengagement;
using SupercellProxy.Networking.Protocol.RoadsideShops;
using SupercellProxy.Networking.Protocol.Sanctuary;
using SupercellProxy.Networking.Protocol.Tutorials;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents decoded <c language="csharp">CommonAvatarDataSnapshot</c> home data.
/// </summary>
public sealed record CommonAvatarDataSnapshot
{
    /// <summary>Gets or sets the account-link reward claims retained by the native offer manager.</summary>
    /// <value>The Hay Day Pop and Supercell ID reward flags read from the <c>LogicOfferManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    [JsonPropertyName("LogicOfferManager")]
    public AccountLinkRewardsSnapshot? AccountLinkRewards { get; init; }

    /// <summary>Gets or sets the retained boat difficulty pool.</summary>
    /// <value>The difficulty pool and its daily reset time read from the <c>BoatOrderManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    public BoatOrderManagerSnapshot? BoatOrderManager { get; init; }
    /// <summary>Gets or sets the saved boat delivery track: milestones completed, points still pending and when the track resets.</summary>
    /// <value>The boat track read from the <c>BoatTrackManager</c> object, or <see langword="null"/> when the home has no boat track saved yet.</value>
    public OrderTrackSnapshot? BoatTrackManager { get; init; }
    /// <summary>Gets or sets the retained booster manager state.</summary>
    /// <value>The active, stored and pending boosters read from the <c>BoosterManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    public BoosterManagerSnapshot? BoosterManager { get; init; }
    /// <summary>Gets or sets the retained calendar-gift collection flags.</summary>
    /// <value>The per-event gift collection state read from the <c>CalendarGiftBoxEventManager</c> object, or <see langword="null"/> when no calendar-gift event was ever saved.</value>
    [JsonPropertyName("CalendarGiftBoxEventManager")]
    public CalendarGiftManagerSnapshot? CalendarGifts { get; init; }

    /// <summary>Gets or sets the saved chain-offer requirements and claims.</summary>
    /// <value>The per-offer progress and claimed rewards read from the <c>ChainOfferManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    public ChainOfferManagerSnapshot? ChainOfferManager { get; init; }
    /// <summary>Gets or sets the saved scheduled events: per-event player data, events already seen, and the event board.</summary>
    /// <value>The scheduled-event state read from the <c>ChronosEvents</c> object, or <see langword="null"/> when the save has no such object.</value>
    public ChronosEventsSnapshot? ChronosEvents { get; init; }

    /// <summary>
    /// Gets or sets the saved creature spawning counters: spawns per rule today and in the current event, failed spawns, and whether a
    /// creature was caught in the current event.
    /// </summary>
    /// <value>The counters read from the <c>CreatureManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    public CreatureManagerSnapshot? CreatureManager { get; init; }

    /// <summary>Gets or sets the saved customization parts the player has seen, used and bought, and the renovator's stock and rerolls.</summary>
    /// <value>The customization state read from the <c>CustomizationManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    public CustomizationManagerSnapshot? CustomizationManager { get; init; }

    /// <summary>Gets or sets the reward choices still waiting for the player and their saved seed.</summary>
    /// <value>The pending decision boxes read from the <c>LogicDecisionBoxManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    [JsonPropertyName("LogicDecisionBoxManager")]
    public DecisionBoxManagerSnapshot? DecisionBoxManager { get; init; }

    /// <summary>Gets or sets the saved decoration sticker book.</summary>
    /// <value>Whether the book was opened and each collection's progress, read from the <c>DecoStickerbookManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    [JsonPropertyName("DecoStickerbookManager")]
    public DecoStickerBookManagerSnapshot? DecoStickerBookManager { get; init; }

    /// <summary>Gets or sets the player's saved part in the decoration (design) event, such as the submitted design, its likes and the intro tutorial.</summary>
    /// <value>The decoration event's id, owner and challenge state read from the <c>DecoEventMgr</c> object, or <see langword="null"/> when the save has no such object.</value>
    [JsonPropertyName("DecoEventMgr")]
    public DecorationEventManagerSnapshot? DecorationEventManager { get; init; }

    /// <summary>Gets or sets the saved decoration-event voting state.</summary>
    /// <value>The voting league, likes and current candidate pair read from the <c>DecoEventVotingMgr</c> object, or <see langword="null"/> when the save has no such object.</value>
    [JsonPropertyName("DecoEventVotingMgr")]
    public DecorationVotingSnapshot? DecorationVoting { get; init; }

    /// <summary>Gets or sets the saved active derby task and its progress.</summary>
    /// <value>The derby task flags, goal counters and limits read from the <c>DerbyManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    public DerbyManagerSnapshot? DerbyManager { get; init; }
    /// <summary>Gets or sets the retained emote state.</summary>
    /// <value>The newest emote ids read from the <c>EmoteManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    public EmoteManagerSnapshot? EmoteManager { get; init; }

    /// <summary>Gets or sets the saved Farm Pass season: its tiers, points, tasks and claimed rewards.</summary>
    /// <value>The Farm Pass state read from the <c>FarmPassManager</c> object, or <see langword="null"/> when the home has never saved a Farm Pass.</value>
    public FarmPassSnapshot? FarmPassManager { get; init; }

    /// <summary>Gets or sets the shared sequence used to identify passengers and help requests.</summary>
    /// <value>The last allocated help id read from the <c>LogicUniqueHelpId</c> object, or <see langword="null"/> when the save has no such object.</value>
    [JsonPropertyName("LogicUniqueHelpId")]
    public HelpIdSequenceSnapshot? HelpIds { get; init; }

    /// <summary>Gets or sets the saved Valley state: board, fuel, quests, notifications and the promotion shop.</summary>
    /// <value>The Valley state read from the <c>MapGameManager</c> object, or <see langword="null"/> when the home has no Valley state saved.</value>
    public MapGameSnapshot? MapGameManager { get; init; }

    /// <summary>Gets or sets the saved Mini Pass instance and active tasks.</summary>
    /// <value>The current Mini Pass and its task slots read from the <c>LogicMiniPassManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    [JsonPropertyName("LogicMiniPassManager")]
    public MiniPassManagerSnapshot? MiniPassManager { get; init; }

    /// <summary>Gets or sets the saved movie tickets (watch-an-ad reward offers) and the state of each.</summary>
    /// <value>The per-placement ticket states read from the <c>MovieTicketManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    public MovieTicketManagerSnapshot? MovieTicketManager { get; init; }

    /// <summary>Gets or sets the saved neighbourhood object state with its active event and the last neighbourhood the player was in.</summary>
    /// <value>The neighbourhood event progress read from the <c>LogicNeighborhoodObjectManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    [JsonPropertyName("LogicNeighborhoodObjectManager")]
    public NeighborhoodObjectManagerSnapshot? NeighborhoodObjectManager { get; init; }

    /// <summary>Gets or sets the received neighborhood donations.</summary>
    /// <value>The uncollected donated goods and their notification flag read from the <c>LogicNeighborhoodRequestsManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    [JsonPropertyName("LogicNeighborhoodRequestsManager")]
    public NeighborhoodRequestsSnapshot? NeighborhoodRequests { get; init; }

    /// <summary>Gets or sets the saved newspaper counters (bought from the newspaper bot today, tools bought today) and its refresh timer.</summary>
    /// <value>The newspaper state read from the <c>NewspaperManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    public NewspaperSnapshot? NewspaperManager { get; init; }

    /// <summary>Gets or sets the saved derby promotion rewards and eligibility.</summary>
    /// <value>The promotion goods, pending Hay Day boxes and weekly box counters read from the <c>PopPromoManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    public PopPromotionManagerSnapshot? PopPromoManager { get; init; }

    /// <summary>Gets or sets the saved re-engagement flow: its coin and diamond reward and the screen the player reached.</summary>
    /// <value>The flow state read from the <c>LogicReEngagementFlowManager</c> object, or <see langword="null"/> when no re-engagement flow was saved.</value>
    [JsonPropertyName("LogicReEngagementFlowManager")]
    public ReengagementFlowSnapshot? ReengagementFlow { get; init; }

    /// <summary>Gets or sets the inventory-owned roadside listing cancellation history.</summary>
    /// <value>The cancellation cooldown, cancellation count and opened mussel count read from the <c>LogicInventory</c> object, or <see langword="null"/> when the save has no such object.</value>
    [JsonPropertyName("LogicInventory")]
    public RoadsideCancellationSnapshot? RoadsideCancellation { get; init; }

    /// <summary>Gets or sets the saved Sanctuary puzzle progression.</summary>
    /// <value>The nonempty and completed animal puzzles read from the <c>SanctuaryManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    [JsonPropertyName("SanctuaryManager")]
    public SanctuaryManagerSnapshot? SanctuaryManager { get; init; }
    /// <summary>Gets or sets the truck-generation daily progression.</summary>
    /// <value>The orders dispatched since the daily reset and the reset time, read from the <c>TruckOrderManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    public TruckOrderManagerSnapshot? TruckOrderManager { get; init; }

    /// <summary>Gets or sets the saved truck delivery track: milestones completed, points still pending and when the track resets.</summary>
    /// <value>The truck track read from the <c>TruckTrackManager</c> object, or <see langword="null"/> when the home has no truck track saved yet.</value>
    public OrderTrackSnapshot? TruckTrackManager { get; init; }
    /// <summary>Gets or sets the saved tutorial group and the flags of the tutorial steps completed.</summary>
    /// <value>The tutorial state read from the <c>TutorialMgr</c> object, or <see langword="null"/> when the save has no such object.</value>
    [JsonPropertyName("TutorialMgr")]
    public TutorialManagerSnapshot? TutorialManager { get; init; }

    /// <summary>Gets or sets the saved production workbench and installed perks.</summary>
    /// <value>The building mastery states and perk flags read from the <c>LogicWorkbenchManager</c> object, or <see langword="null"/> when the save has no such object.</value>
    [JsonPropertyName("LogicWorkbenchManager")]
    public WorkbenchSnapshot? Workbench { get; init; }
}
