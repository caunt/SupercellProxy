using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>
/// Defines the Avatar Ranking Entry contract.
/// </summary>
/// <summary>
/// Defines the Rank contract.
/// </summary>
/// <summary>
/// Defines the Score contract.
/// </summary>
/// <summary>
/// Defines the Rank Value contract.
/// </summary>
/// <summary>
/// Defines the Avatar Id contract.
/// </summary>
/// <summary>
/// Defines the Name contract.
/// </summary>
/// <summary>
/// Defines the Level contract.
/// </summary>
/// <summary>
/// Defines the Home Id contract.
/// </summary>
public sealed record AvatarRankingEntry(
    int Rank,
    int Score,
    int RankValue,
    [property: System.Text.Json.Serialization.JsonPropertyName("AvatarId")] LongIdentifier AvatarIdentifier,
    string? Name,
    int Level,
    [property: System.Text.Json.Serialization.JsonPropertyName("HomeId")] LongIdentifier HomeIdentifier
)
{

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AvatarRankingEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadLongIdentifier(),
            stream.ReadOptionalString(),
            stream.ReadVariableInt(),
            stream.ReadLongIdentifier()
        );
    }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AvatarRankingEntry[]? DecodeEntries(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = stream.ReadVariableInt();

        if (count < -1 || count > (stream.Length - stream.Position) / 24)
            throw new InvalidDataException(message: "The player-ranking count is invalid.");

        if (count < 0)
            return null;

        AvatarRankingEntry[] entries = new AvatarRankingEntry[count];

        for (int index = 0; index < count; index++)
            entries[index] = Decode(stream);

        return entries;
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public static void EncodeEntries(MessageStream stream, AvatarRankingEntry[]? entries)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (entries is null)
            stream.WriteVariableInt(valueToWrite: -1);
        else
            stream.WriteArray(entries, static (output, entry) => entry.Encode(output));
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(Rank);
        stream.WriteVariableInt(Score);
        stream.WriteVariableInt(RankValue);
        stream.WriteLongIdentifier(AvatarIdentifier);
        stream.WriteOptionalString(Name);
        stream.WriteVariableInt(Level);
        stream.WriteLongIdentifier(HomeIdentifier);
    }
}
