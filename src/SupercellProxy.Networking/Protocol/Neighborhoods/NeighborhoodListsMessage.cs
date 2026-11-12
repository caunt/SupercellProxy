using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Clientbound neighbourhood ranking lists: scored neighbourhood profiles and two lists of scored ids; all three are empty in most
/// captures.
/// </summary>
public sealed record NeighborhoodListsMessage : CodecMessage<NeighborhoodListsMessage>, ICodecProvider<NeighborhoodListsMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodListsMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.NullableArray(NeighborhoodIdValue.Codec, CollectionCount.VarInt.WithStrictAbsence()),
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
            }),
        trailingDataMessage: "The neighborhood lists contain trailing data.");

    /// <summary>
    /// Gets or sets ids with a score each, sent second and sorted from the highest score in the captures; likely a player ranking, not confirmed.
    /// </summary>
    /// <value>The second list on the wire, with a VarInt count; <see langword="null"/> when the server writes it as absent.</value>
    public NeighborhoodIdValue[]? FirstEntries { get; init; }

    /// <summary>Gets or sets the neighbourhood profiles with a score each, sorted from the highest score in the captures.</summary>
    /// <value>The third list on the wire, with a VarInt count; <see langword="null"/> when the server writes it as absent.</value>
    public NeighborhoodHeaderEntryValue[]? Profiles { get; init; }

    /// <summary>Gets or sets ids with a number each, sent first and not sorted in the captures; their meaning is not identified.</summary>
    /// <value>The first list on the wire, with a VarInt count; <see langword="null"/> when the server writes it as absent.</value>
    public NeighborhoodIdValue[]? SecondEntries { get; init; }

    /// <summary>Gets or sets the number sent last, 0 when the lists are empty; its meaning is not identified.</summary>
    /// <value>The VarInt that ends the message; any bytes after it fail the decode.</value>
    public int Value { get; init; }

    /// <summary>Returns only the message name, without the lists.</summary>
    /// <returns>Always <c>NeighborhoodListsMessage</c>, so logs never show the listed ids or profiles.</returns>
    public override string ToString()
    {
        return nameof(NeighborhoodListsMessage);
    }
}
