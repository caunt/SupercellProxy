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
    public static PlayerRankings23708Message Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;
        AvatarRankingEntry[]? entries = AvatarRankingEntry.DecodeEntries(stream);

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The ranking response has trailing data.")
            : new PlayerRankings23708Message { Entries = entries };
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        AvatarRankingEntry.EncodeEntries(stream, Entries);

        return new MessageContainer(identifier, version, stream);
    }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(PlayerRankings23708Message);
    }
}
