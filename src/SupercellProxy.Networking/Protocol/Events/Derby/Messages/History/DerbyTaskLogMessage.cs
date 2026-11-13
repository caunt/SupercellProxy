using System.Collections.Immutable;

using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.History;

/// <summary>Returns both native member collections for the neighborhood's derby task log.</summary>
public sealed record DerbyTaskLogMessage : CodecMessage<DerbyTaskLogMessage>, ICodecProvider<DerbyTaskLogMessage>
{
    /// <inheritdoc />
    public static ICodec<DerbyTaskLogMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.NullableArray(DerbyMemberTaskLogEntry.Codec, CollectionCount.VarInt.WithStrictAbsence()),
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
            }),
        trailingDataMessage: "The derby message has trailing data.");

    /// <summary>Gets or sets the first optional collection in native order.</summary>
    /// <value>The member rows of the first list in the reply, or <see langword="null"/> when native wrote the -1 absent count; which tab of the log it fills is not yet identified.</value>
    public ImmutableArray<DerbyMemberTaskLogEntry>? FirstEntries { get; init; }

    /// <summary>Gets or sets the second optional collection in native order.</summary>
    /// <value>The member rows of the second list, read right after <see cref="FirstEntries"/>, or <see langword="null"/> when native wrote the -1 absent count.</value>
    public ImmutableArray<DerbyMemberTaskLogEntry>? SecondEntries { get; init; }

    /// <summary>Gets or sets the first value following the two collections.</summary>
    /// <value>The var-int written after <see cref="SecondEntries"/>; its meaning is not yet identified, so it is carried through unchanged.</value>
    public int Value0 { get; init; }

    /// <summary>Gets or sets the second value following the two collections.</summary>
    /// <value>The var-int that ends the reply, written after <see cref="Value0"/>; its meaning is not yet identified.</value>
    public int Value1 { get; init; }

    /// <summary>Omits member details from diagnostic text.</summary>
    /// <returns>The type name <c>DerbyTaskLogMessage</c> alone, so member identifiers and names never reach logs.</returns>
    public override string ToString()
    {
        return nameof(DerbyTaskLogMessage);
    }
}
