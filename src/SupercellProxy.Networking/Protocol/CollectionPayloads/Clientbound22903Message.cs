using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CollectionPayloads;

/// <summary>
/// Defines the Clientbound22903 Message contract.
/// </summary>
public sealed record Clientbound22903Message : IMessage
{
    /// <summary>
    /// Gets the Entries Present value.
    /// </summary>
    public bool EntriesPresent { get; init; }
    /// <summary>
    /// Gets the Event Id value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("EventId")]
    public int EventId { get; init; }

    /// <summary>
    /// Gets the Trailing Value value.
    /// </summary>
    public int TrailingValue { get; init; }

    /// <summary>
    /// Gets the Value value.
    /// </summary>
    public long Value { get; init; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static Clientbound22903Message Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int eventId = stream.ReadVarInt();
        int count = stream.ReadVarInt();

        if (count > 0)
            throw new NotSupportedException(message: "Populated clientbound event collections are not implemented.");

        Clientbound22903Message message = new()
        {
            EventId = eventId,
            EntriesPresent = count is 0,
            Value = stream.ReadVarLong(),
            TrailingValue = stream.ReadVarInt(),
        };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The clientbound event collection has trailing data.")
            : message;
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteVarInt(EventId);
        stream.WriteVarInt(EntriesPresent ? 0 : -1);
        stream.WriteVarLong(Value);
        stream.WriteVarInt(TrailingValue);
    }
}
