using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Customization;

/// <summary>Describes a purchase discount for one named customization category.</summary>
public sealed record RenovatorDiscountDefinition
{
    /// <summary>Gets or sets how many percent are taken off the price of parts in the category (saved as <c>discount</c>).</summary>
    /// <value>A percentage applied to coin, diamond and voucher prices; 0 by default.</value>
    [JsonPropertyName("discount")]
    public int DiscountPercentage { get; init; }

    /// <summary>Gets or sets the asset-qualified category name.</summary>
    /// <value>A name of the form <c>Asset.Category</c>, read from <c>part</c>; <see langword="null"/> when absent.</value>
    [JsonPropertyName("part")]
    public string? Part { get; init; }
}
