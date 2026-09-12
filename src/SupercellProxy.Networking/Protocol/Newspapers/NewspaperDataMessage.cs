using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>
/// Defines the Newspaper Data Message contract.
/// </summary>
public sealed record NewspaperDataMessage : IMessage
{

    /// <summary>
    /// Gets the Placements value.
    /// </summary>
    public NewspaperPlacement[]? Placements { get; init; }
    /// <summary>
    /// Gets the Stories value.
    /// </summary>
    public NewspaperStoryEntry[]? Stories { get; init; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static NewspaperDataMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;
        int storyCount = stream.ReadVariableInt();

        if (storyCount > (stream.Length - stream.Position) / 24)
            throw new InvalidDataException(message: "The newspaper story collection is truncated.");

        NewspaperStoryEntry[]? stories = storyCount < 0 ? null : new NewspaperStoryEntry[storyCount];

        if (stories is not null)
        {
            for (int index = 0; index < stories.Length; index++)
                stories[index] = NewspaperStoryEntry.Decode(stream);
        }

        int placementCount = stream.ReadVariableInt();

        if (placementCount > (stream.Length - stream.Position) / 14)
            throw new InvalidDataException(message: "The newspaper placement collection is truncated.");

        NewspaperPlacement[]? placements = placementCount < 0 ? null : new NewspaperPlacement[placementCount];

        if (placements is not null)
        {
            for (int index = 0; index < placements.Length; index++)
                placements[index] = NewspaperPlacement.Decode(stream);
        }

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The newspaper data has trailing bytes.")
            : new NewspaperDataMessage { Stories = stories, Placements = placements };
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(Stories?.Length ?? -1);

        if (Stories is not null)
        {
            foreach (NewspaperStoryEntry entry in Stories)
                entry.Encode(stream);
        }

        stream.WriteVariableInt(Placements?.Length ?? -1);

        if (Placements is not null)
        {
            foreach (NewspaperPlacement entry in Placements)
                entry.Encode(stream);
        }

        return new MessageContainer(identifier, version, stream);
    }
}
