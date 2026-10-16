using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Timing;
using SupercellProxy.Networking.Protocol.Town.Services;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>One saved town service entry.</summary>
public sealed record TownServiceEntrySnapshot : ExtensibleDocument
{
    /// <summary>Gets whether an event shortened the service.</summary>
    public bool? EventSpeedUp { get; init; }

    /// <summary>Gets whether the owner collected the help, which started the service.</summary>
    [JsonPropertyName("HC")]
    public bool? HelpCollected { get; init; }

    /// <summary>Gets the unique service-help request id.</summary>
    [JsonPropertyName("HID")]
    public int? HelpId { get; init; }

    /// <summary>Gets the kind of help requested for the service: town visitors, neighbourhood or Farm Pass.</summary>
    [JsonPropertyName("HRD")]
    public int? HelpKind { get; init; }

    /// <summary>Gets the home id of the player who helped.</summary>
    [JsonPropertyName("ReviverAvatarId")]
    public TownServiceHelperSnapshot? Helper { get; init; }

    /// <summary>Gets whether the owner thanked the helper.</summary>
    [JsonPropertyName("HG")]
    public bool? HelperThanked { get; init; }

    /// <summary>Gets the saved service id.</summary>
    [JsonPropertyName("ID")]
    public int Id { get; init; }

    /// <summary>Gets whether service has started.</summary>
    public bool Started { get; init; }

    /// <summary>Gets the running service's native timer.</summary>
    public TimerSnapshot? Timer { get; init; }

    /// <summary>Gets whether the service uses the tutorial instant-completion price.</summary>
    [JsonPropertyName("TutorialInstantCompletePrice")]
    public bool? UsesTutorialInstantCompletePrice { get; init; }
}
