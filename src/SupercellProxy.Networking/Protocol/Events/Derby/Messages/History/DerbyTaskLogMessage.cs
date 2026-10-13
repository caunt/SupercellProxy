using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.History;

/// <summary>Returns both native member collections for the neighborhood's derby task log.</summary>
public sealed record DerbyTaskLogMessage : CodecMessage<DerbyTaskLogMessage>, ICodecProvider<DerbyTaskLogMessage>
{
    /// <inheritdoc />
    public static ICodec<DerbyTaskLogMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.NullableArray(DerbyMemberTaskLogEntry.Codec, CollectionCount.VarInt.WithStrictAbsence()),
            static message => message.FirstEntries,
            Codecs.NullableArray(DerbyMemberTaskLogEntry.Codec, CollectionCount.VarInt.WithStrictAbsence()),
            static message => message.SecondEntries,
            Codecs.VarInt,
            static message => message.Value0,
            Codecs.VarInt,
            static message => message.Value1,
            static (firstEntries, secondEntries, value0, value1) => new DerbyTaskLogMessage
            {
                FirstEntries = firstEntries,
                SecondEntries = secondEntries,
                Value0 = value0,
                Value1 = value1,
            }
        ),
        trailingDataMessage: "The derby message has trailing data."
    );

    /// <summary>Gets the first optional collection in native order.</summary>
    public DerbyMemberTaskLogEntry[]? FirstEntries { get; init; }
    /// <summary>Gets the second optional collection in native order.</summary>
    public DerbyMemberTaskLogEntry[]? SecondEntries { get; init; }
    /// <summary>Gets the first value following the two collections.</summary>
    public int Value0 { get; init; }
    /// <summary>Gets the second value following the two collections.</summary>
    public int Value1 { get; init; }

    /// <summary>Omits member details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DerbyTaskLogMessage);
    }
}
