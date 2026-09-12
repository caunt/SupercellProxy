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
public sealed record NeighborhoodIdentifierValue([property: System.Text.Json.Serialization.JsonPropertyName("Id")] LongIdentifier Identifier, int Value);
