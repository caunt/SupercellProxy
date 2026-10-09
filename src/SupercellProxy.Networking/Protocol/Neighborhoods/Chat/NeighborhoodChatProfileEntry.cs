using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Retains one tagged, optional identifier in a chat sender's profile.</summary>
public sealed record NeighborhoodChatProfileEntry(int Kind, LongId? Identifier)
{
    internal static NeighborhoodChatProfileEntry Decode(MessageStream stream)
    {
        return new(stream.ReadVarInt(), stream.ReadOptionalLongId());
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Kind);
        stream.WriteOptionalLongId(Identifier);
    }
}
