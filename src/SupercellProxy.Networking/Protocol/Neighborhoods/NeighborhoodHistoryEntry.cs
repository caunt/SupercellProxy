using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Defines the Neighborhood History Entry contract.
/// </summary>
public sealed record NeighborhoodHistoryEntry
{
    private const int ValueCount = 8;

    /// <summary>
    /// Gets the Flag value.
    /// </summary>
    public bool Flag { get; init; }

    /// <summary>
    /// Gets the Values value.
    /// </summary>
    public int[] Values { get; init; } = new int[ValueCount];

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static NeighborhoodHistoryEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new() { Values = stream.ReadVariableIntArray(ValueCount), Flag = stream.ReadBoolean() };
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (Values.Length != ValueCount)
            throw new InvalidDataException(message: "Invalid neighborhood history scalar count.");

        foreach (int value in Values)
            stream.WriteVariableInt(value);

        stream.WriteBoolean(Flag);
    }
}
