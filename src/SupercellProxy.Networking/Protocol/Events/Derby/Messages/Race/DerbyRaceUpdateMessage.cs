using SupercellProxy.Networking.Protocol.Events.Derby.Race;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.Race;

/// <summary>Updates one neighborhood race entry using the native signed presence discriminator.</summary>
public sealed record DerbyRaceUpdateMessage(int Value0, int Value1, int EntryDiscriminator, DerbyRaceEntry? Entry) : CodecMessage<DerbyRaceUpdateMessage>, ICodecProvider<DerbyRaceUpdateMessage>
{
    /// <inheritdoc />
    public static ICodec<DerbyRaceUpdateMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.VarInt,
            static message => message.Value0,
            Codecs.VarInt,
            static message => message.Value1,
            RaceEntry.Codec,
            static message => new RaceEntry(message.EntryDiscriminator, message.Entry),
            static (value0, value1, raceEntry) => new DerbyRaceUpdateMessage(value0, value1, raceEntry.EntryDiscriminator, raceEntry.Entry)
        ),
        trailingDataMessage: "The derby message has trailing data."
    );

    /// <summary>Omits neighborhood details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DerbyRaceUpdateMessage);
    }

    // A non-negative marker precedes the entry; a negative one means no entry.
    private readonly record struct RaceEntry(int EntryDiscriminator, DerbyRaceEntry? Entry)
    {
        public static ICodec<RaceEntry> Codec { get; } = Codecs.Create(
            static stream =>
            {
                int entryDiscriminator = stream.ReadVarInt();

                return new RaceEntry(entryDiscriminator, entryDiscriminator < 0 ? null : DerbyRaceEntry.Codec.Read(stream));
            },
            static (stream, segment) =>
            {
                if ((segment.EntryDiscriminator >= 0) != (segment.Entry is not null))
                    throw new InvalidDataException(message: "The derby race update marker does not match its entry.");

                stream.WriteVarInt(segment.EntryDiscriminator);

                if (segment.Entry is not null)
                    DerbyRaceEntry.Codec.Write(stream, segment.Entry);
            }
        );
    }
}
