using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Retains one Valley shop reward resource or token price.</summary>
public sealed record MapGameRewardAmountSnapshot
{
    /// <summary>Gets or sets the resource quantity.</summary>
    /// <value>How many of the resource the reward gives or the price asks, saved as <c>Value</c>.</value>
    [JsonPropertyName("Value")]
    public int Amount { get; init; }

    /// <summary>Gets or sets the resource data identifier.</summary>
    /// <value>The global id of the resource row, saved as <c>ID</c>: a Valley coin row for a price, any reward row for a reward.</value>
    [JsonPropertyName("ID")]
    public int GlobalId { get; init; }
}
