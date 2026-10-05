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
    public static NeighborhoodMembersMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;
        int count = stream.ReadVariableInt();

        if (count is < -1 or > 1000)
            throw new InvalidDataException(message: "The neighborhood member count is invalid.");

        NeighborhoodMemberEntry[]? members = count < 0 ? null : new NeighborhoodMemberEntry[count];

        if (members is not null)
        {
            for (int index = 0; index < members.Length; index++)
                members[index] = NeighborhoodMemberEntry.Decode(stream);
        }

        int unknownTail = stream.ReadVariableInt();

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The neighborhood member list has trailing data.")
            : new NeighborhoodMembersMessage { Members = members, UnknownTail = unknownTail };
    }

    /// <summary>Encodes the optional member list and trailing value.</summary>
    public MessageStream ToStream()
    {
        if (Members?.Length is > 1000)
            throw new InvalidDataException(message: "The neighborhood member count is invalid.");

        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(Members?.Length ?? -1);

        if (Members is not null)
        {
            foreach (NeighborhoodMemberEntry member in Members)
                member.Encode(stream);
        }

        stream.WriteVariableInt(UnknownTail);

        return stream;
    }

    /// <summary>Omits member details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(NeighborhoodMembersMessage);
    }
}
