using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>One saved town service entry.</summary>
public sealed record TownServiceEntrySnapshot : ExtensibleDocument
{
    /// <summary>Gets the saved HID value.</summary>
    [System.Text.Json.Serialization.JsonPropertyName("HID")]
    public int? Hid { get; init; }

    /// <summary>Gets the saved HRD value.</summary>
    [System.Text.Json.Serialization.JsonPropertyName("HRD")]
    public int? Hrd { get; init; }

    /// <summary>Gets the saved service identifier.</summary>
    [System.Text.Json.Serialization.JsonPropertyName("ID")]
    public int Identifier { get; init; }

    /// <summary>Gets whether service has started.</summary>
    public bool Started { get; init; }
}
