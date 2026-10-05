using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Members;

/// <summary>Clientbound member list for a requested neighborhood.</summary>
public sealed record NeighborhoodMembersMessage : IMessage
{
    /// <summary>Gets the optional ordered list of members.</summary>
    public NeighborhoodMemberEntry[]? Members { get; init; }

    /// <summary>Gets the trailing native value whose purpose is not yet established.</summary>
    public int UnknownTail { get; init; }

    /// <summary>Decodes the optional member list and trailing value.</summary>
    public static NeighborhoodMembersMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = stream.ReadVarInt();

        if (count is < -1 or > 1000)
            throw new InvalidDataException(message: "The neighborhood member count is invalid.");

        NeighborhoodMemberEntry[]? members = count < 0 ? null : new NeighborhoodMemberEntry[count];

        if (members is not null)
        {
            for (int index = 0; index < members.Length; index++)
                members[index] = NeighborhoodMemberEntry.Decode(stream);
        }

        int unknownTail = stream.ReadVarInt();

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The neighborhood member list has trailing data.")
            : new NeighborhoodMembersMessage { Members = members, UnknownTail = unknownTail };
    }

    /// <summary>Encodes the optional member list and trailing value.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (Members?.Length is > 1000)
            throw new InvalidDataException(message: "The neighborhood member count is invalid.");

        stream.WriteVarInt(Members?.Length ?? -1);

        if (Members is not null)
        {
            foreach (NeighborhoodMemberEntry member in Members)
                member.Encode(stream);
        }

        stream.WriteVarInt(UnknownTail);
    }

    /// <summary>Omits member details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(NeighborhoodMembersMessage);
    }
}
