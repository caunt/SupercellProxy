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
    public int EventIdentifier { get; init; }

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
    public static Clientbound22903Message Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;
        int eventIdentifier = stream.ReadVariableInt();
        int count = stream.ReadVariableInt();

        if (count > 0)
            throw new NotSupportedException(message: "Populated clientbound event collections are not implemented.");

        Clientbound22903Message message = new()
        {
            EventIdentifier = eventIdentifier,
            EntriesPresent = count is 0,
            Value = stream.ReadVariableLong(),
            TrailingValue = stream.ReadVariableInt(),
        };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The clientbound event collection has trailing data.")
            : message;
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(EventIdentifier);
        stream.WriteVariableInt(EntriesPresent ? 0 : -1);
        stream.WriteVariableLong(Value);
        stream.WriteVariableInt(TrailingValue);

        return new MessageContainer(identifier, version, stream);
    }
}
