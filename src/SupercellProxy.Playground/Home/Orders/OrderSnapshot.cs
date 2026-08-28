using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">OrderSnapshot</c> home data.
/// </summary>
internal sealed record OrderSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">Lvl</c> value.
    /// </summary>
    public int Lvl { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Datas</c> value.
    /// </summary>
    public int[] Datas { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">Amounts</c> value.
    /// </summary>
    public int[] Amounts { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">Cash</c> value.
    /// </summary>
    public int Cash { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Exp</c> value.
    /// </summary>
    public int Exp { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Voucher</c> value.
    /// </summary>
    public int Voucher { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CashExpMultiplier</c> value.
    /// </summary>
    public int CashExpMultiplier { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Receiver</c> value.
    /// </summary>
    public int Receiver { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Data</c> value.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement> Data { get; init; } =
        new Dictionary<string, JsonElement>(StringComparer.Ordinal);
}
