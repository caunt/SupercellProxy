using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>OrderSnapshot</c> home data.
/// </summary>
public sealed record OrderSnapshot
{
    /// <summary>
    /// Gets or sets the <c>Lvl</c> value.
    /// </summary>
    public int Lvl { get; init; }

    /// <summary>
    /// Gets or sets the <c>Datas</c> value.
    /// </summary>
    public int[] Datas { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>Amounts</c> value.
    /// </summary>
    public int[] Amounts { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>Cash</c> value.
    /// </summary>
    public int Cash { get; init; }

    /// <summary>
    /// Gets or sets the <c>Exp</c> value.
    /// </summary>
    public int Exp { get; init; }

    /// <summary>
    /// Gets or sets the <c>Voucher</c> value.
    /// </summary>
    public int Voucher { get; init; }

    /// <summary>
    /// Gets or sets the <c>CashExpMultiplier</c> value.
    /// </summary>
    public int CashExpMultiplier { get; init; }

    /// <summary>
    /// Gets or sets the <c>Receiver</c> value.
    /// </summary>
    public int Receiver { get; init; }

    /// <summary>
    /// Gets or sets the <c>Data</c> value.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement> Data { get; init; } =
        new Dictionary<string, JsonElement>(StringComparer.Ordinal);
}
