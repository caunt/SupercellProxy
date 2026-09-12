namespace SupercellProxy.Networking.Json;

/// <summary>Preserves document fields whose schema has not been decoded, entirely within the JSON codec.</summary>
public sealed record PreservedDocument : ExtensibleDocument;
