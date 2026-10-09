using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Retains one Valley shop reward resource or token price.</summary>
public sealed record MapGameRewardAmountSnapshot
{
    /// <summary>Gets the resource quantity.</summary>
    [JsonPropertyName("Value")]
    public int Amount { get; init; }

    /// <summary>Gets the resource data identifier.</summary>
    [JsonPropertyName("ID")]
    public int GlobalId { get; init; }
}
