using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Customization;

/// <summary>Describes a purchase discount for one named customization category.</summary>
public sealed record RenovatorDiscountDefinition
{
    /// <summary>How many percent are taken off the price of parts in the category (saved as <c>discount</c>).</summary>
    [JsonPropertyName("discount")]
    public int DiscountPercentage { get; init; }

    /// <summary>Gets the asset-qualified category name.</summary>
    [JsonPropertyName("part")]
    public string? Part { get; init; }
}
