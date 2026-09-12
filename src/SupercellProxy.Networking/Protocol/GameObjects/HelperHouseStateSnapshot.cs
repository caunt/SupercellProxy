using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>Represents decoded HelperHouseStateSnapshot state.</summary>
public sealed record HelperHouseStateSnapshot : ExtensibleDocument
{
    /// <summary>Gets the OfferDeclined value.</summary>
    [JsonPropertyName("OfferDeclined")]
    public bool OfferDeclined { get; init; }

    /// <summary>Gets the State value.</summary>
    [JsonPropertyName("State")]
    public int State { get; init; }

}
