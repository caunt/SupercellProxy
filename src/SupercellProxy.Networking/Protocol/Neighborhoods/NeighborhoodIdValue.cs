namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Defines the Neighborhood Id Value contract.
/// </summary>
/// <summary>
/// Defines the Id contract.
/// </summary>
/// <summary>
/// Defines the Value contract.
/// </summary>
public sealed record NeighborhoodIdValue([property: System.Text.Json.Serialization.JsonPropertyName("Id")] LongId Id, int Value);
