using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>Clientbound featured decoration designs for one event group.</summary>
public sealed record FeaturingDesignListMessage : IMessage
{
    /// <summary>Gets the batch timestamp in milliseconds.</summary>
    public long BatchTimestampMilliseconds { get; init; }

    /// <summary>Gets the optional featured-design list.</summary>
    public FeaturingDesignEntry?[]? Entries { get; init; }

    /// <summary>Gets the decoration event id.</summary>
    public int EventId { get; init; }

    /// <summary>Gets the decoration event variant id.</summary>
    public int EventVariantId { get; init; }

    /// <summary>Gets the featuring group id.</summary>
    public int GroupId { get; init; }

    /// <summary>Decodes a featured-design list message.</summary>
    public static FeaturingDesignListMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        FeaturingDesignEntry?[]? entries = null;

        if (stream.ReadBoolean())
        {
            int count = stream.ReadVarInt();

            if (count is < 0 or > 1024)
                throw new InvalidDataException(message: "The featured-design list count is invalid.");

            entries = new FeaturingDesignEntry?[count];

            for (int index = 0; index < entries.Length; index++)
                entries[index] = stream.ReadBoolean() ? FeaturingDesignEntry.Decode(stream) : null;
        }

        FeaturingDesignListMessage message = new()
        {
            Entries = entries,
            BatchTimestampMilliseconds = stream.ReadVarLong(),
            GroupId = stream.ReadVarInt(),
            EventId = stream.ReadVarInt(),
            EventVariantId = stream.ReadVarInt(),
        };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The featured-design list has trailing data.")
            : message;
    }

    /// <summary>Encodes a featured-design list message.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteBoolean(Entries is not null);

        if (Entries is { } entries)
        {
            if (entries.Length > 1024)
                throw new InvalidDataException(message: "The featured-design list count is invalid.");

            stream.WriteVarInt(entries.Length);

            foreach (FeaturingDesignEntry? entry in entries)
            {
                stream.WriteBoolean(entry is not null);
                entry?.Encode(stream);
            }
        }

        stream.WriteVarLong(BatchTimestampMilliseconds);
        stream.WriteVarInt(GroupId);
        stream.WriteVarInt(EventId);
        stream.WriteVarInt(EventVariantId);
    }
}
