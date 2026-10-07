using SupercellProxy.Networking.Protocol.Events.Derby.Race;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.Race;

/// <summary>Updates one neighborhood race entry using the native signed presence discriminator.</summary>
public sealed record DerbyRaceUpdateMessage(int Value0, int Value1, int EntryDiscriminator, DerbyRaceEntry? Entry) : IMessage
{
    /// <summary>Decodes the two selectors and optional entry, retaining the wire discriminator.</summary>
    public static DerbyRaceUpdateMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int value0 = stream.ReadVarInt();
        int value1 = stream.ReadVarInt();
        int discriminator = stream.ReadVarInt();
        DerbyRaceUpdateMessage message = new(value0, value1, discriminator, discriminator < 0 ? null : DerbyRaceEntry.Decode(stream));
        DerbyMessageCodec.RequireEnd(stream);

        return message;
    }

    /// <summary>Encodes the update without converting its signed marker into a boolean.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if ((EntryDiscriminator >= 0) != (Entry is not null))
            throw new InvalidDataException(message: "The derby race update marker does not match its entry.");

        stream.WriteVarInt(Value0);
        stream.WriteVarInt(Value1);
        stream.WriteVarInt(EntryDiscriminator);
        Entry?.Encode(stream);
    }

    /// <summary>Omits neighborhood details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DerbyRaceUpdateMessage);
    }
}
