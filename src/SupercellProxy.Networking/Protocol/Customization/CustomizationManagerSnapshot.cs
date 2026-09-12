namespace SupercellProxy.Networking.Protocol.Customization;

/// <summary>
/// Represents decoded <c language="csharp">CustomizationManagerSnapshot</c> home data.
/// </summary>
public sealed record CustomizationManagerSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">StockSeconds</c> value.
    /// </summary>
    public int StockSeconds { get; init; }
}
