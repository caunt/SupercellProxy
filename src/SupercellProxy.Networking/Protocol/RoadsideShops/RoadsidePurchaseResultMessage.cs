using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Defines the Roadside Purchase Result Message contract.
/// </summary>
/// <summary>
/// Defines the Buyer Id contract.
/// </summary>
/// <summary>
/// Defines the Home Owner Id contract.
/// </summary>
/// <summary>
/// Defines the Status contract.
/// </summary>
/// <summary>
/// Defines the Quantity contract.
/// </summary>
/// <summary>
/// Defines the Price contract.
/// </summary>
/// <summary>
/// Defines the Slot Index contract.
/// </summary>
/// <summary>
/// Defines the Context Value contract.
/// </summary>
/// <summary>
/// Defines the Item Global Id contract.
/// </summary>
public sealed record RoadsidePurchaseResultMessage(
    [property: System.Text.Json.Serialization.JsonPropertyName("BuyerId")] LongIdentifier? BuyerIdentifier,
    [property: System.Text.Json.Serialization.JsonPropertyName("HomeOwnerId")] LongIdentifier? HomeOwnerIdentifier,
    int Status,
    int Quantity,
    int Price,
    int SlotIndex,
    int ContextValue,
    [property: System.Text.Json.Serialization.JsonPropertyName("ItemGlobalId")] int ItemGlobalIdentifier
) : IMessage
{

    /// <summary>
    /// Provides the Buyer Update Status value or operation.
    /// </summary>
    public const int BuyerUpdateStatus = 3;
    /// <summary>
    /// Provides the Successful Status value or operation.
    /// </summary>
    public const int SuccessfulStatus = 0;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static RoadsidePurchaseResultMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;

        RoadsidePurchaseResultMessage message = new(
            stream.ReadOptionalLongIdentifier(),
            stream.ReadOptionalLongIdentifier(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt()
        );

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "Roadside purchase result has trailing data.")
            : message;
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteOptionalLongIdentifier(BuyerIdentifier);
        stream.WriteOptionalLongIdentifier(HomeOwnerIdentifier);
        stream.WriteVariableInt(Status);
        stream.WriteVariableInt(Quantity);
        stream.WriteVariableInt(Price);
        stream.WriteVariableInt(SlotIndex);
        stream.WriteVariableInt(ContextValue);
        stream.WriteVariableInt(ItemGlobalIdentifier);

        return new MessageContainer(identifier, version, stream);
    }
}
