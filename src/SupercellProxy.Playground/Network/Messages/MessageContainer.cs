using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages;

/// <summary>
/// Represents <c>MessageContainer</c>.
/// </summary>
public record MessageContainer(ushort Id, ushort Version, MessageStream Payload);
