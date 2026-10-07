using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>
/// Defines the neighborhood object leaderboard list message contract.
/// </summary>
public sealed record NeighborhoodObjectLeaderboardListMessage : IMessage
{
    /// <summary>
    /// Gets the Entries value.
    /// </summary>
    public AvatarRankingEntry[]? Entries { get; init; }
    /// <summary>
    /// Gets the Value0 value.
    /// </summary>
    public int Value0 { get; init; }

    /// <summary>
    /// Gets the Value1 value.
    /// </summary>
    public int Value1 { get; init; }

    /// <summary>
    /// Gets the Value2 value.
    /// </summary>
    public int Value2 { get; init; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static NeighborhoodObjectLeaderboardListMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        NeighborhoodObjectLeaderboardListMessage message = new()
        {
            Value0 = stream.ReadVarInt(),
            Value1 = stream.ReadVarInt(),
            Value2 = stream.ReadVarInt(),
            Entries = AvatarRankingEntry.DecodeEntries(stream),
        };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The player-ranking page has trailing data.")
            : message;
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteVarInt(Value0);
        stream.WriteVarInt(Value1);
        stream.WriteVarInt(Value2);
        AvatarRankingEntry.EncodeEntries(stream, Entries);
    }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(NeighborhoodObjectLeaderboardListMessage);
    }
}
