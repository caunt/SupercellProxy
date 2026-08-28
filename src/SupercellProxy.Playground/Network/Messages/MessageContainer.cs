using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages;

/// <summary>
/// Represents <c language="csharp">MessageContainer</c>.
/// </summary>
internal sealed record MessageContainer(ushort Id, ushort Version, MessageStream Payload);
