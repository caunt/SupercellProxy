using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Timing;
using SupercellProxy.Networking.Protocol.Town.Services;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>One saved town service entry.</summary>
public sealed record TownServiceEntrySnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets whether an event shortened the service.</summary>
    /// <value><see langword="true"/> if a town event's service-time percentage below 100 applied when the service started; otherwise, <see langword="false"/>, or <see langword="null"/> when the save omits it.</value>
    public bool? EventSpeedUp { get; init; }

    /// <summary>Gets or sets whether the owner collected the help, which started the service.</summary>
    /// <value><see langword="true"/> after the owner collected a helper's work; otherwise, <see langword="false"/>, or <see langword="null"/> when the save omits <c>HC</c>.</value>
    [JsonPropertyName("HC")]
    public bool? HelpCollected { get; init; }

    /// <summary>Gets or sets the unique service-help request id.</summary>
    /// <value>Help id of the owner's help request, saved as <c>HID</c>; 0 once the help is cleared, or <see langword="null"/> when no help was ever requested.</value>
    [JsonPropertyName("HID")]
    public int? HelpId { get; init; }

    /// <summary>Gets or sets the kind of help requested for the service: town visitors, neighbourhood or Farm Pass.</summary>
    /// <value>1 for town visitors, 2 for the neighbourhood and 3 for Farm Pass helpers; 0 after the help is cleared, or <see langword="null"/> when no help was requested.</value>
    [JsonPropertyName("HRD")]
    public int? HelpKind { get; init; }

    /// <summary>Gets or sets the home id of the player who helped.</summary>
    /// <value>Helper's home id split into high and low words under <c>ReviverAvatarId</c>, or <see langword="null"/> while nobody has helped.</value>
    [JsonPropertyName("ReviverAvatarId")]
    public TownServiceHelperSnapshot? Helper { get; init; }

    /// <summary>Gets or sets whether the owner thanked the helper.</summary>
    /// <value><see langword="true"/> once the owner sent thanks for the help; otherwise, <see langword="false"/>, or <see langword="null"/> when the save omits <c>HG</c>.</value>
    [JsonPropertyName("HG")]
    public bool? HelperThanked { get; init; }

    /// <summary>Gets or sets the saved service id.</summary>
    /// <value>Data global id of the service building type that owns the entry, which the client writes when it books or saves the service; 0 by default.</value>
    [JsonPropertyName("ID")]
    public int Id { get; init; }

    /// <summary>Gets or sets whether service has started.</summary>
    /// <value><see langword="true"/> if the service is running, either because the owner spent the goods or collected the help; otherwise, <see langword="false"/>.</value>
    /// <remarks>An entry that only holds a booking has the value <see langword="false"/>.</remarks>
    public bool Started { get; init; }

    /// <summary>Gets or sets the running service's native timer.</summary>
    /// <value>Service duration in seconds and the timer ticks still left, or <see langword="null"/> for a booking that has not started.</value>
    public TimerSnapshot? Timer { get; init; }

    /// <summary>Gets or sets whether the service uses the tutorial instant-completion price.</summary>
    /// <value><see langword="true"/> for a service booked during a tutorial, whose speed-up costs no diamonds; otherwise, <see langword="false"/>, or <see langword="null"/> when the save omits it.</value>
    [JsonPropertyName("TutorialInstantCompletePrice")]
    public bool? UsesTutorialInstantCompletePrice { get; init; }
}
