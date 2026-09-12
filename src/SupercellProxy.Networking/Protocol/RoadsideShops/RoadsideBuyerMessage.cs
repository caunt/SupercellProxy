using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Defines the Roadside Buyer Message contract.
/// </summary>
/// <summary>
/// Defines the Slot Index contract.
/// </summary>
/// <summary>
/// Defines the Buyer Id contract.
/// </summary>
public sealed record RoadsideBuyerMessage(int SlotIndex, [property: System.Text.Json.Serialization.JsonPropertyName("BuyerId")] LongIdentifier BuyerIdentifier) : IMessage
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static RoadsideBuyerMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        RoadsideBuyerMessage result = new(container.Payload.ReadVariableInt(), container.Payload.ReadLongIdentifier());

        return container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "The roadside buyer update has trailing data.")
            : result;
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(SlotIndex);
        stream.WriteLongIdentifier(BuyerIdentifier);

        return new MessageContainer(identifier, version, stream);
    }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(RoadsideBuyerMessage);
    }
}
