using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Defines the Neighborhood Lists Message contract.
/// </summary>
public sealed record NeighborhoodListsMessage : CodecMessage<NeighborhoodListsMessage>, ICodecProvider<NeighborhoodListsMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodListsMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.NullableArray(NeighborhoodIdValue.Codec, CollectionCount.VarInt.WithStrictAbsence()),
            static message => message.SecondEntries,
            Codecs.NullableArray(NeighborhoodIdValue.Codec, CollectionCount.VarInt.WithStrictAbsence()),
            static message => message.FirstEntries,
            Codecs.NullableArray(NeighborhoodHeaderEntryValue.Codec, CollectionCount.VarInt.WithStrictAbsence()),
            static message => message.Profiles,
            Codecs.VarInt,
            static message => message.Value,
            static (secondEntries, firstEntries, profiles, value) => new NeighborhoodListsMessage
            {
                SecondEntries = secondEntries,
                FirstEntries = firstEntries,
                Profiles = profiles,
                Value = value,
            }
        ),
        trailingDataMessage: "The neighborhood lists contain trailing data."
    );

    /// <summary>
    /// Gets the First Entries value.
    /// </summary>
    public NeighborhoodIdValue[]? FirstEntries { get; init; }

    /// <summary>
    /// Gets the Profiles value.
    /// </summary>
    public NeighborhoodHeaderEntryValue[]? Profiles { get; init; }

    /// <summary>
    /// Gets the Second Entries value.
    /// </summary>
    public NeighborhoodIdValue[]? SecondEntries { get; init; }

    /// <summary>
    /// Gets the Value value.
    /// </summary>
    public int Value { get; init; }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(NeighborhoodListsMessage);
    }
}
