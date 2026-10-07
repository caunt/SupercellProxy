using SupercellProxy.Networking.Protocol.Events.Derby.Race;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.Race;

/// <summary>Returns the optional ordered derby neighborhood rankings.</summary>
public sealed record DerbyRankingsMessage(DerbyRankingEntry[]? Entries) : IMessage
{
    /// <summary>Decodes the complete nullable ranking collection.</summary>
    public static DerbyRankingsMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        DerbyRankingsMessage message = new(DerbyMessageCodec.ReadOptionalArray(stream, DerbyRankingEntry.Decode));
        DerbyMessageCodec.RequireEnd(stream);

        return message;
    }

    /// <summary>Encodes the complete nullable ranking collection.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        DerbyMessageCodec.WriteOptionalArray(stream, Entries, static (writer, entry) => entry.Encode(writer));
    }

    /// <summary>Omits neighborhood details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DerbyRankingsMessage);
    }
}
