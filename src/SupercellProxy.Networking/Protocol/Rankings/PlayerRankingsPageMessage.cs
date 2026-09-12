using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>
/// Defines the Player Rankings Page Message contract.
/// </summary>
public sealed record PlayerRankingsPageMessage : IMessage
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
    public static PlayerRankingsPageMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;

        PlayerRankingsPageMessage message = new()
        {
            Value0 = stream.ReadVariableInt(),
            Value1 = stream.ReadVariableInt(),
            Value2 = stream.ReadVariableInt(),
            Entries = AvatarRankingEntry.DecodeEntries(stream),
        };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The player-ranking page has trailing data.")
            : message;
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(Value0);
        stream.WriteVariableInt(Value1);
        stream.WriteVariableInt(Value2);
        AvatarRankingEntry.EncodeEntries(stream, Entries);

        return new MessageContainer(identifier, version, stream);
    }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(PlayerRankingsPageMessage);
    }
}
