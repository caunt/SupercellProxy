using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>
/// Represents <c language="csharp">MessageContainer</c>.
/// </summary>
public sealed record MessageContainer([property: System.Text.Json.Serialization.JsonPropertyName("Id")] ushort Identifier, ushort Version, MessageStream Payload);
