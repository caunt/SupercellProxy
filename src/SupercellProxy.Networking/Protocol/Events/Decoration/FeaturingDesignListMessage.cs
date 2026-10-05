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

    /// <summary>Gets the decoration event identifier.</summary>
    public int EventIdentifier { get; init; }

    /// <summary>Gets the decoration event variant identifier.</summary>
    public int EventVariantIdentifier { get; init; }

    /// <summary>Gets the featuring group identifier.</summary>
    public int GroupIdentifier { get; init; }

    /// <summary>Decodes a featured-design list message.</summary>
    public static FeaturingDesignListMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;
        FeaturingDesignEntry?[]? entries = null;

        if (stream.ReadBoolean())
        {
            int count = stream.ReadVariableInt();

            if (count is < 0 or > 1024)
                throw new InvalidDataException(message: "The featured-design list count is invalid.");

            entries = new FeaturingDesignEntry?[count];

            for (int index = 0; index < entries.Length; index++)
                entries[index] = stream.ReadBoolean() ? FeaturingDesignEntry.Decode(stream) : null;
        }

        FeaturingDesignListMessage message = new()
        {
            Entries = entries,
            BatchTimestampMilliseconds = stream.ReadVariableLong(),
            GroupIdentifier = stream.ReadVariableInt(),
            EventIdentifier = stream.ReadVariableInt(),
            EventVariantIdentifier = stream.ReadVariableInt(),
        };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The featured-design list has trailing data.")
            : message;
    }

    /// <summary>Encodes a featured-design list message.</summary>
    public MessageStream ToStream()
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteBoolean(Entries is not null);

        if (Entries is { } entries)
        {
            if (entries.Length > 1024)
                throw new InvalidDataException(message: "The featured-design list count is invalid.");

            stream.WriteVariableInt(entries.Length);

            foreach (FeaturingDesignEntry? entry in entries)
            {
                stream.WriteBoolean(entry is not null);
                entry?.Encode(stream);
            }
        }

        stream.WriteVariableLong(BatchTimestampMilliseconds);
        stream.WriteVariableInt(GroupIdentifier);
        stream.WriteVariableInt(EventIdentifier);
        stream.WriteVariableInt(EventVariantIdentifier);

        return stream;
    }
}
