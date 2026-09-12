namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Represents decoded <c language="csharp">NeighborhoodObjectManagerSnapshot</c> home data.
/// </summary>
public sealed record NeighborhoodObjectManagerSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">ActiveEventId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("ActiveEventId")]
    public int ActiveEventIdentifier { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">State</c> value.
    /// </summary>
    public NeighborhoodObjectStateSnapshot State { get; init; } = new();
}
