using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>Names the player's farm, as the native tutorial name prompt and the settings name button do.</summary>
public sealed record ChangeHomeNameMessage(string Name) : IMessage
{
    /// <summary>Decodes the requested farm name.</summary>
    public static ChangeHomeNameMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new ChangeHomeNameMessage(stream.ReadString());
    }

    /// <summary>Encodes the requested farm name.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteString(Name);
    }
}
