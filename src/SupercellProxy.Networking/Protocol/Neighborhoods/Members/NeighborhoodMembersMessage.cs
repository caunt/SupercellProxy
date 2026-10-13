using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Members;

/// <summary>Clientbound member list for a requested neighborhood.</summary>
public sealed record NeighborhoodMembersMessage : CodecMessage<NeighborhoodMembersMessage>, ICodecProvider<NeighborhoodMembersMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodMembersMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.NullableArray(NeighborhoodMemberSummary.Codec, CollectionCount.AtMost(maximum: 1000).WithStrictAbsence()),
            static message => message.Members,
            Codecs.VarInt,
            static message => message.UnknownTail,
            static (members, unknownTail) => new NeighborhoodMembersMessage
            {
                Members = members,
                UnknownTail = unknownTail,
            }
        ),
        trailingDataMessage: "The neighborhood member list has trailing data."
    );

    /// <summary>Gets the optional ordered list of members.</summary>
    public NeighborhoodMemberSummary[]? Members { get; init; }

    /// <summary>Gets the trailing native value whose purpose is not yet established.</summary>
    public int UnknownTail { get; init; }

    /// <summary>Omits member details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(NeighborhoodMembersMessage);
    }
}
