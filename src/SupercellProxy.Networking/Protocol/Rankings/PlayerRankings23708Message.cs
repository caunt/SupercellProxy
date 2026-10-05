using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>
/// Defines the Player Rankings23708 Message contract.
/// </summary>
public sealed record PlayerRankings23708Message : IMessage
{
    /// <summary>
    /// Gets the Entries value.
    /// </summary>
    public AvatarRankingEntry[]? Entries { get; init; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static PlayerRankings23708Message Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        AvatarRankingEntry[]? entries = AvatarRankingEntry.DecodeEntries(stream);

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The ranking response has trailing data.")
            : new PlayerRankings23708Message { Entries = entries };
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
        return nameof(PlayerRankings23708Message);
    }
}
