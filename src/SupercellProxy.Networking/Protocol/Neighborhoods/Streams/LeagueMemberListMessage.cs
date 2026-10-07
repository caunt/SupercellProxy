using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Events.Derby.Messages;
using SupercellProxy.Networking.Protocol.Events.Derby.Race;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams;

/// <summary>Clientbound league-member list with two native selection values.</summary>
public sealed record LeagueMemberListMessage : IMessage
{
    /// <summary>Gets the optional neighborhood race entries.</summary>
    public DerbyRaceEntry[]? Entries { get; init; }

    /// <summary>Gets the optional entry count, including existing empty-page callers.</summary>
    public int? EntryCount { get => Entries?.Length ?? field; init; }

    /// <summary>Gets the first native stream selector.</summary>
    public int FirstSelector { get; init; }

    /// <summary>Gets the second native stream selector.</summary>
    public int SecondSelector { get; init; }

    /// <summary>Gets the optional native derby race supplement.</summary>
    public DerbyBoardSupplement? Supplement { get; init; }

    /// <summary>Gets the seconds timestamp carried by this stream page.</summary>
    public int TimestampSeconds { get; init; }

    /// <summary>Decodes the complete native race page.</summary>
    public static LeagueMemberListMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int timestamp = stream.ReadVarInt();
        int firstSelector = stream.ReadVarInt();
        int secondSelector = stream.ReadVarInt();
        int count = stream.ReadVarInt();

        if (count < -1 || count > stream.Length - stream.Position)
            throw new InvalidDataException(message: "The avatar-stream entry count is invalid.");

        DerbyRaceEntry[]? entries = count < 0 ? null : new DerbyRaceEntry[count];

        if (entries is not null)
        {
            for (int index = 0; index < entries.Length; index++)
                entries[index] = DerbyRaceEntry.Decode(stream);
        }

        LeagueMemberListMessage message = new()
        {
            TimestampSeconds = timestamp,
            FirstSelector = firstSelector,
            SecondSelector = secondSelector,
            EntryCount = count < 0 ? null : count,
            Entries = entries,
            Supplement = stream.ReadBoolean() ? DerbyBoardSupplement.Decode(stream) : null,
        };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The avatar-stream page has trailing data.")
            : message;
    }

    /// <summary>Encodes the complete native race page.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (EntryCount is < 0)
            throw new InvalidDataException(message: "The avatar-stream entry count is invalid.");

        if (EntryCount is > 0 && Entries is null)
            throw new NotSupportedException(message: "The declared race entry count requires concrete entry data.");

        stream.WriteVarInt(TimestampSeconds);
        stream.WriteVarInt(FirstSelector);
        stream.WriteVarInt(SecondSelector);
        stream.WriteVarInt(EntryCount ?? -1);

        if (Entries is not null)
        {
            foreach (DerbyRaceEntry entry in Entries)
                entry.Encode(stream);
        }

        stream.WriteBoolean(Supplement is not null);
        Supplement?.Encode(stream);
    }

    /// <summary>Omits stream contents from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(LeagueMemberListMessage);
    }
}
