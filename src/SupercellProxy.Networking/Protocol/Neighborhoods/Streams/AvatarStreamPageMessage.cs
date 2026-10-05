using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams;

/// <summary>Clientbound avatar-stream page with two native selection values.</summary>
public sealed record AvatarStreamPageMessage : IMessage
{
    /// <summary>Gets the count of the optional entry list; only its empty form is currently understood.</summary>
    public int? EntryCount { get; init; }

    /// <summary>Gets the first native stream selector.</summary>
    public int FirstSelector { get; init; }

    /// <summary>Gets the second native stream selector.</summary>
    public int SecondSelector { get; init; }

    /// <summary>Gets the seconds timestamp carried by this stream page.</summary>
    public int TimestampSeconds { get; init; }

    /// <summary>Decodes the proven empty-page form of an avatar-stream message.</summary>
    public static AvatarStreamPageMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;
        int timestamp = stream.ReadVariableInt();
        int firstSelector = stream.ReadVariableInt();
        int secondSelector = stream.ReadVariableInt();
        int count = stream.ReadVariableInt();

        if (count is < -1 or > 1024)
            throw new InvalidDataException(message: "The avatar-stream entry count is invalid.");

        if (count > 0)
            throw new NotSupportedException(message: "Nonempty avatar-stream entries have an unconfirmed layout.");

        if (stream.ReadBoolean())
            throw new NotSupportedException(message: "The avatar-stream supplemental entry has an unconfirmed layout.");

        AvatarStreamPageMessage message = new()
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

    /// <summary>Encodes the proven empty-page form of an avatar-stream message.</summary>
    public MessageStream ToStream()
    {
        if (EntryCount is < 0 or > 1024)
            throw new InvalidDataException(message: "The avatar-stream entry count is invalid.");

        if (EntryCount is > 0)
            throw new NotSupportedException(message: "Nonempty avatar-stream entries have an unconfirmed layout.");

        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(TimestampSeconds);
        stream.WriteVariableInt(FirstSelector);
        stream.WriteVariableInt(SecondSelector);
        stream.WriteVariableInt(EntryCount ?? -1);
        stream.WriteBoolean(value: false);

        return stream;
    }

    /// <summary>Omits stream contents from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(AvatarStreamPageMessage);
    }
}
