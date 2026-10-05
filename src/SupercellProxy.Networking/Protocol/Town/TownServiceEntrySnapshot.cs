using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>One saved town service entry.</summary>
public sealed record TownServiceEntrySnapshot : ExtensibleDocument
{
    /// <summary>Gets whether an event shortened the service.</summary>
    public bool? EventSpeedUp { get; init; }

    /// <summary>Gets the unique service-help request id.</summary>
    [System.Text.Json.Serialization.JsonPropertyName("HID")]
    public int? HelpId { get; init; }

    /// <summary>Gets the saved HRD value.</summary>
    [System.Text.Json.Serialization.JsonPropertyName("HRD")]
    public int? Hrd { get; init; }

    /// <summary>Gets the saved service id.</summary>
    [System.Text.Json.Serialization.JsonPropertyName("ID")]
    public int Id { get; init; }

    /// <summary>Gets whether service has started.</summary>
    public bool Started { get; init; }

    /// <summary>Gets the running service's native timer.</summary>
    public TimerSnapshot? Timer { get; init; }

    /// <summary>Gets whether the service uses the tutorial instant-completion price.</summary>
    [System.Text.Json.Serialization.JsonPropertyName("TutorialInstantCompletePrice")]
    public bool? UsesTutorialInstantCompletePrice { get; init; }
}
