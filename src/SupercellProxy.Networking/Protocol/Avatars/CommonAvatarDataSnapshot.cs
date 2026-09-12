using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Creatures;
using SupercellProxy.Networking.Protocol.Customization;
using SupercellProxy.Networking.Protocol.Emotes;
using SupercellProxy.Networking.Protocol.Events.Chronos;
using SupercellProxy.Networking.Protocol.Events.Decoration;
using SupercellProxy.Networking.Protocol.FarmPass;
using SupercellProxy.Networking.Protocol.Gifts;
using SupercellProxy.Networking.Protocol.MapGame;
using SupercellProxy.Networking.Protocol.MovieTickets;
using SupercellProxy.Networking.Protocol.Neighborhoods;
using SupercellProxy.Networking.Protocol.Newspapers;
using SupercellProxy.Networking.Protocol.Orders;
using SupercellProxy.Networking.Protocol.Reengagement;
using SupercellProxy.Networking.Protocol.Tutorials;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents decoded <c language="csharp">CommonAvatarDataSnapshot</c> home data.
/// </summary>
public sealed record CommonAvatarDataSnapshot
{

    /// <summary>
    /// Gets the Boat Track Manager value.
    /// </summary>
    public OrderTrackSnapshot? BoatTrackManager { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ChronosEvents</c> value.
    /// </summary>
    public ChronosEventsSnapshot? ChronosEvents { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CreatureManager</c> value.
    /// </summary>
    public CreatureManagerSnapshot? CreatureManager { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CustomizationManager</c> value.
    /// </summary>
    public CustomizationManagerSnapshot? CustomizationManager { get; init; }

    /// <summary>
    /// Gets the Decision Box Manager value.
    /// </summary>
    [JsonPropertyName("LogicDecisionBoxManager")]
    public DecisionBoxManagerSnapshot? DecisionBoxManager { get; init; }

    /// <summary>
    /// <para>Gets the retained decoration-event manager state.</para>
    /// </summary>
    [JsonPropertyName("DecoEventMgr")]
    public DecorationEventManagerSnapshot? DecorationEventManager { get; init; }

    /// Gets the retained emote state used by home creation gates.
    public EmoteManagerSnapshot? EmoteManager { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">FarmPassManager</c> value.
    /// </summary>
    public FarmPassSnapshot? FarmPassManager { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">MapGameManager</c> value.
    /// </summary>
    public MapGameSnapshot? MapGameManager { get; init; }

    /// <summary>
    /// Gets the Movie Ticket Manager value.
    /// </summary>
    public MovieTicketManagerSnapshot? MovieTicketManager { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">NeighborhoodObjectManager</c> value.
    /// </summary>
    [JsonPropertyName("LogicNeighborhoodObjectManager")]
    public NeighborhoodObjectManagerSnapshot? NeighborhoodObjectManager { get; init; }

    /// <summary>
    /// Gets the Newspaper Manager value.
    /// </summary>
    public NewspaperSnapshot? NewspaperManager { get; init; }

    /// <summary>
    /// Gets the Reengagement Flow value.
    /// </summary>
    [JsonPropertyName("LogicReEngagementFlowManager")]
    public ReengagementFlowSnapshot? ReengagementFlow { get; init; }
    /// <summary>
    /// Gets the Truck Track Manager value.
    /// </summary>
    public OrderTrackSnapshot? TruckTrackManager { get; init; }

    /// <summary>
    /// Gets the Tutorial Manager value.
    /// </summary>
    [JsonPropertyName("TutorialMgr")]
    public TutorialManagerSnapshot? TutorialManager { get; init; }
}
