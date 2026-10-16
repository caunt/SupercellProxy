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
    /// <summary>Gets account-link reward claims retained by the native offer manager.</summary>
    [JsonPropertyName("LogicOfferManager")]
    public AccountLinkRewardsSnapshot? AccountLinkRewards { get; init; }

    /// <summary>Gets the retained boat difficulty pool.</summary>
    public BoatOrderManagerSnapshot? BoatOrderManager { get; init; }
    /// <summary>Gets the saved boat delivery track: milestones completed, points still pending and when the track resets.</summary>
    public OrderTrackSnapshot? BoatTrackManager { get; init; }
    /// <summary>Gets the retained booster manager state.</summary>
    public BoosterManagerSnapshot? BoosterManager { get; init; }
    /// <summary>Gets the retained calendar-gift collection flags.</summary>
    [JsonPropertyName("CalendarGiftBoxEventManager")]
    public CalendarGiftManagerSnapshot? CalendarGifts { get; init; }

    /// <summary>Gets saved chain-offer requirements and claims.</summary>
    public ChainOfferManagerSnapshot? ChainOfferManager { get; init; }
    /// <summary>Gets the saved scheduled events: per-event player data, events already seen, and the event board.</summary>
    public ChronosEventsSnapshot? ChronosEvents { get; init; }

    /// <summary>
    /// Gets the saved creature spawning counters: spawns per rule today and in the current event, failed spawns, and whether a
    /// creature was caught in the current event.
    /// </summary>
    public CreatureManagerSnapshot? CreatureManager { get; init; }

    /// <summary>Gets the saved customization parts the player has seen, used and bought, and the renovator's stock and rerolls.</summary>
    public CustomizationManagerSnapshot? CustomizationManager { get; init; }

    /// <summary>Gets the reward choices still waiting for the player and the seed their contents are drawn with.</summary>
    [JsonPropertyName("LogicDecisionBoxManager")]
    public DecisionBoxManagerSnapshot? DecisionBoxManager { get; init; }

    /// <summary>
    /// <para>Gets the retained decoration-event manager state.</para>
    /// </summary>
    [JsonPropertyName("DecoEventMgr")]
    public DecorationEventManagerSnapshot? DecorationEventManager { get; init; }

    /// <summary>Gets the saved decoration-event voting state.</summary>
    [JsonPropertyName("DecoEventVotingMgr")]
    public DecorationVotingSnapshot? DecorationVoting { get; init; }

    /// <summary>Gets the saved active derby task and its progress.</summary>
    public DerbyManagerSnapshot? DerbyManager { get; init; }
    /// <summary>Gets the retained emote state used by home creation gates.</summary>
    public EmoteManagerSnapshot? EmoteManager { get; init; }

    /// <summary>Gets the saved Farm Pass season: its tiers, points, tasks and claimed rewards.</summary>
    public FarmPassSnapshot? FarmPassManager { get; init; }

    /// <summary>Gets the shared sequence used to identify passengers and help requests.</summary>
    [JsonPropertyName("LogicUniqueHelpId")]
    public HelpIdSequenceSnapshot? HelpIds { get; init; }

    /// <summary>Gets the saved Valley state: board, fuel, quests, notifications and the promotion shop.</summary>
    public MapGameSnapshot? MapGameManager { get; init; }

    /// <summary>Gets the saved Mini Pass instance and active tasks.</summary>
    [JsonPropertyName("LogicMiniPassManager")]
    public MiniPassManagerSnapshot? MiniPassManager { get; init; }

    /// <summary>Gets the saved movie tickets (watch-an-ad reward offers) and the state of each.</summary>
    public MovieTicketManagerSnapshot? MovieTicketManager { get; init; }

    /// <summary>Gets the saved neighbourhood object state with its active event and the last neighbourhood the player was in.</summary>
    [JsonPropertyName("LogicNeighborhoodObjectManager")]
    public NeighborhoodObjectManagerSnapshot? NeighborhoodObjectManager { get; init; }

    /// <summary>Gets received neighborhood donations.</summary>
    [JsonPropertyName("LogicNeighborhoodRequestsManager")]
    public NeighborhoodRequestsSnapshot? NeighborhoodRequests { get; init; }

    /// <summary>Gets the saved newspaper counters (bought from the newspaper bot today, tools bought today) and its refresh timer.</summary>
    public NewspaperSnapshot? NewspaperManager { get; init; }

    /// <summary>Gets saved derby promotion rewards and eligibility.</summary>
    public PopPromotionManagerSnapshot? PopPromoManager { get; init; }

    /// <summary>Gets the saved re-engagement flow: its coin and diamond reward and the screen the player reached.</summary>
    [JsonPropertyName("LogicReEngagementFlowManager")]
    public ReengagementFlowSnapshot? ReengagementFlow { get; init; }

    /// <summary>Inventory-owned roadside listing cancellation history.</summary>
    [JsonPropertyName("LogicInventory")]
    public RoadsideCancellationSnapshot? RoadsideCancellation { get; init; }

    /// <summary>Gets the saved Sanctuary puzzle progression.</summary>
    [JsonPropertyName("SanctuaryManager")]
    public SanctuaryManagerSnapshot? SanctuaryManager { get; init; }
    /// <summary>Gets the truck-generation daily progression.</summary>
    public TruckOrderManagerSnapshot? TruckOrderManager { get; init; }

    /// <summary>Gets the saved truck delivery track: milestones completed, points still pending and when the track resets.</summary>
    public OrderTrackSnapshot? TruckTrackManager { get; init; }
    /// <summary>Gets the saved tutorial group and the flags of the tutorial steps completed.</summary>
    [JsonPropertyName("TutorialMgr")]
    public TutorialManagerSnapshot? TutorialManager { get; init; }

    /// <summary>Gets the saved production workbench and installed perks.</summary>
    [JsonPropertyName("LogicWorkbenchManager")]
    public WorkbenchSnapshot? Workbench { get; init; }
}
