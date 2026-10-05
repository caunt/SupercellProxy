using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Proxy;

/// <summary>Named result returned by ReadForwardMessageAsync.</summary>
internal readonly record struct DecodedMessageFrame(IMessage Message, MessageContainer Original);
