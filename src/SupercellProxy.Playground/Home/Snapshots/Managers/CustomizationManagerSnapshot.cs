namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>CustomizationManagerSnapshot</c> home data.
/// </summary>
public sealed record CustomizationManagerSnapshot
{
    /// <summary>
    /// Gets or sets the <c>StockSeconds</c> value.
    /// </summary>
    public int StockSeconds { get; init; }
}
