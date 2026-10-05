using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>Clientbound decoration-gallery lists for an identified home.</summary>
public sealed record DecorationGalleryDataMessage : IMessage
{
    /// <summary>Gets the count of the first optional list; only an empty list is currently understood.</summary>
    public int? FirstListCount { get; init; }

    /// <summary>Gets the home whose gallery data is being supplied, when present.</summary>
    public LongId? HomeId { get; init; }

    /// <summary>Gets the count of the second optional list; only an empty list is currently understood.</summary>
    public int? SecondListCount { get; init; }

    /// <summary>Decodes the two optional lists followed by an optional home id.</summary>
    public static DecorationGalleryDataMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        DecorationGalleryDataMessage message = new()
        {
            FirstListCount = ReadListCount(stream),
            SecondListCount = ReadListCount(stream),
            HomeId = stream.ReadBoolean() ? stream.ReadLongId() : null,
        };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The decoration-gallery data has trailing data.")
            : message;
    }

    /// <summary>Encodes the proven empty-list form of a decoration-gallery response.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        WriteListCount(stream, FirstListCount);
        WriteListCount(stream, SecondListCount);
        stream.WriteBoolean(HomeId is not null);

        if (HomeId is { } homeId)
            stream.WriteLongId(homeId);
    }

    /// <summary>Omits the private home id from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DecorationGalleryDataMessage);
    }

    private static int? ReadListCount(MessageStream stream)
    {
        if (!stream.ReadBoolean())
            return null;

        int count = stream.ReadVarInt();

        return count is < 0 or > 1024
            ? throw new InvalidDataException(message: "The decoration-gallery list count is invalid.")
            : count != 0
            ? throw new NotSupportedException(message: "Nonempty decoration-gallery lists have an unconfirmed element layout.")
            : count;
    }

    private static void WriteListCount(MessageStream stream, int? count)
    {
        stream.WriteBoolean(count is not null);

        if (count is not { } value)
            return;

        if (value is < 0 or > 1024)
            throw new InvalidDataException(message: "The decoration-gallery list count is invalid.");

        if (value != 0)
            throw new NotSupportedException(message: "Nonempty decoration-gallery lists have an unconfirmed element layout.");

        stream.WriteVarInt(value);
    }
}
