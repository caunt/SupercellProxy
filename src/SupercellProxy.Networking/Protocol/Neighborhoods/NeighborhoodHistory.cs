using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Defines the Neighborhood History contract.
/// </summary>
public sealed record NeighborhoodHistory
{
    private const int MaximumEntryCount = 10_000;

    /// <summary>
    /// Gets the Entries value.
    /// </summary>
    public NeighborhoodHistoryEntry[]? Entries { get; init; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static NeighborhoodHistory Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = stream.ReadVariableInt();

        if (count is < -1 or > MaximumEntryCount)
            throw new InvalidDataException(message: "Invalid neighborhood history count.");

        NeighborhoodHistoryEntry[]? entries = count < 0 ? null : new NeighborhoodHistoryEntry[count];

        if (entries is not null)
        {
            for (int index = 0; index < entries.Length; index++)
                entries[index] = NeighborhoodHistoryEntry.Decode(stream);
        }

        return new NeighborhoodHistory { Entries = entries };
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(Entries?.Length ?? -1);

        if (Entries is not null)
        {
            foreach (NeighborhoodHistoryEntry entry in Entries)
                entry.Encode(stream);
        }
    }
}
