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
    public static NewspaperDataMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int storyCount = stream.ReadVarInt();

        if (storyCount > (stream.Length - stream.Position) / 24)
            throw new InvalidDataException(message: "The newspaper story collection is truncated.");

        NewspaperStoryEntry[]? stories = storyCount < 0 ? null : new NewspaperStoryEntry[storyCount];

        if (stories is not null)
        {
            for (int index = 0; index < stories.Length; index++)
                stories[index] = NewspaperStoryEntry.Decode(stream);
        }

        int placementCount = stream.ReadVarInt();

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
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteVarInt(Stories?.Length ?? -1);

        if (Stories is not null)
        {
            foreach (NewspaperStoryEntry entry in Stories)
                entry.Encode(stream);
        }

        stream.WriteVarInt(Placements?.Length ?? -1);

        if (Placements is not null)
        {
            foreach (NewspaperPlacement entry in Placements)
                entry.Encode(stream);
        }
    }
}
