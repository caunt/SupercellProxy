namespace SupercellProxy.Networking.Protocol.MessageEncoding;

internal sealed record MessageIdHistory(MessageRegistryEntry Entry, ProtocolIdHistory Ids);
