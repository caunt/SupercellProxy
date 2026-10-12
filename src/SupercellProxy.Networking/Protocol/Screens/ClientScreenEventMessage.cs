using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Screens;

/// <summary>Reports that the client opened or closed a screen element, such as a visited farm's roadside shop.</summary>
public sealed record ClientScreenEventMessage(string Origin, string Action, string Screen, int Value, string Element) : IMessage
{
    /// <summary>Decodes the origin, action, screen, value and element in native field order.</summary>
    public static ClientScreenEventMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadString(), stream.ReadString(), stream.ReadString(), stream.ReadVarInt(), stream.ReadString());
    }

    /// <summary>Encodes the origin, action, screen, value and element in native field order.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteString(Origin);
        stream.WriteString(Action);
        stream.WriteString(Screen);
        stream.WriteVarInt(Value);
        stream.WriteString(Element);
    }
}
