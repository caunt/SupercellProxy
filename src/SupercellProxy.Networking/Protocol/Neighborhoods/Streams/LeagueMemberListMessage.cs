using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams;

/// <summary>Clientbound league-member list with two native selection values.</summary>
public sealed record LeagueMemberListMessage : IMessage
{
    /// <summary>Gets the count of the optional entry list; only its empty form is currently understood.</summary>
    public int? EntryCount { get; init; }

    /// <summary>Gets the first native stream selector.</summary>
    public int FirstSelector { get; init; }

    /// <summary>Gets the second native stream selector.</summary>
    public int SecondSelector { get; init; }

    /// <summary>Gets the seconds timestamp carried by this stream page.</summary>
    public int TimestampSeconds { get; init; }

    /// <summary>Decodes the proven empty-list form of a league-member list message.</summary>
    public static LeagueMemberListMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int timestamp = stream.ReadVarInt();
        int firstSelector = stream.ReadVarInt();
        int secondSelector = stream.ReadVarInt();
        int count = stream.ReadVarInt();

        if (count is < -1 or > 1024)
            throw new InvalidDataException(message: "The avatar-stream entry count is invalid.");

        if (count > 0)
            throw new NotSupportedException(message: "Nonempty avatar-stream entries have an unconfirmed layout.");

        if (stream.ReadBoolean())
            throw new NotSupportedException(message: "The avatar-stream supplemental entry has an unconfirmed layout.");

        LeagueMemberListMessage message = new()
        {
            TimestampSeconds = timestamp,
            FirstSelector = firstSelector,
            SecondSelector = secondSelector,
            EntryCount = count < 0 ? null : count,
        };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The avatar-stream page has trailing data.")
            : message;
    }

    /// <summary>Encodes the proven empty-list form of a league-member list message.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (EntryCount is < 0 or > 1024)
            throw new InvalidDataException(message: "The avatar-stream entry count is invalid.");

        if (EntryCount is > 0)
            throw new NotSupportedException(message: "Nonempty avatar-stream entries have an unconfirmed layout.");

        stream.WriteVarInt(TimestampSeconds);
        stream.WriteVarInt(FirstSelector);
        stream.WriteVarInt(SecondSelector);
        stream.WriteVarInt(EntryCount ?? -1);
        stream.WriteBoolean(value: false);
    }

    /// <summary>Omits stream contents from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(LeagueMemberListMessage);
    }
}
