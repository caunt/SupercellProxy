using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Defines the Roadside Listing Buyer Message contract.
/// </summary>
/// <summary>
/// Defines the Slot Index contract.
/// </summary>
/// <summary>
/// Defines the Buyer Id contract.
/// </summary>
/// <summary>
/// Defines the Home Owner Id contract.
/// </summary>
public sealed record RoadsideListingBuyerMessage(
    int SlotIndex,
    [property: System.Text.Json.Serialization.JsonPropertyName("BuyerId")] LongIdentifier BuyerIdentifier,
    [property: System.Text.Json.Serialization.JsonPropertyName("HomeOwnerId")] LongIdentifier HomeOwnerIdentifier
)
    : IMessage
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static RoadsideListingBuyerMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        RoadsideListingBuyerMessage result = new(container.Payload.ReadVariableInt(), container.Payload.ReadLongIdentifier(), container.Payload.ReadLongIdentifier());

        return container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "Roadside listing buyer update has trailing data.")
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
        stream.WriteLongIdentifier(HomeOwnerIdentifier);

        return new MessageContainer(identifier, version, stream);
    }
}
