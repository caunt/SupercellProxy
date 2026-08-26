namespace SupercellProxy.Playground.Network.Messages;

internal sealed record MessageRegistryEntry(
    ushort Version,
    Type Type,
    Func<MessageContainer, IMessage> Factory
);
