namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">CustomizationManagerSnapshot</c> home data.
/// </summary>
internal sealed record CustomizationManagerSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">StockSeconds</c> value.
    /// </summary>
    public int StockSeconds { get; init; }
}
