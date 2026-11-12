using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Members;

/// <summary>Clientbound member list for a requested neighborhood.</summary>
public sealed record NeighborhoodMembersMessage : CodecMessage<NeighborhoodMembersMessage>, ICodecProvider<NeighborhoodMembersMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodMembersMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.NullableArray(NeighborhoodMemberSummary.Codec, CollectionCount.AtMost(maximum: 1000).WithStrictAbsence()),
            static message => message.Members,
            Codecs.VarInt,
            static message => message.UnknownTail,
            static (members, unknownTail) => new NeighborhoodMembersMessage
            {
                Members = members,
                UnknownTail = unknownTail,
            }),
        trailingDataMessage: "The neighborhood member list has trailing data.");

    /// <summary>Gets or sets the optional ordered list of members.</summary>
    /// <value>Up to 1000 members of the requested neighbourhood in the order the server sent them, or <see langword="null"/> when the server wrote the list as absent.</value>
    public NeighborhoodMemberSummary[]? Members { get; init; }

    /// <summary>Gets or sets the trailing native value whose purpose is not yet established.</summary>
    /// <value>The VarInt that ends the message, after the member list; any bytes after it fail the decode.</value>
    public int UnknownTail { get; init; }

    /// <summary>Omits member details from diagnostic text.</summary>
    /// <returns>Always <c>NeighborhoodMembersMessage</c>, so logs never show farm names or ids.</returns>
    public override string ToString()
    {
        return nameof(NeighborhoodMembersMessage);
    }
}
