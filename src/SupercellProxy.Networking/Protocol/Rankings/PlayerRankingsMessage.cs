using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>
/// Defines the Player Rankings Message contract.
/// </summary>
public sealed record PlayerRankingsMessage : IMessage
{
    /// <summary>
    /// Gets the Entries value.
    /// </summary>
    public AvatarRankingEntry[]? Entries { get; init; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static PlayerRankingsMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        AvatarRankingEntry[]? entries = AvatarRankingEntry.DecodeEntries(stream);

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The player-ranking response has trailing data.")
            : new PlayerRankingsMessage { Entries = entries };
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        AvatarRankingEntry.EncodeEntries(stream, Entries);
    }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(PlayerRankingsMessage);
    }
}
