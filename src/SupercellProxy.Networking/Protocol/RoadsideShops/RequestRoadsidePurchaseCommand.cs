using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Defines the Request Roadside Purchase Command contract.
/// </summary>
/// <summary>
/// Defines the Slot Index contract.
/// </summary>
/// <summary>
/// Defines the Item Global Id contract.
/// </summary>
public sealed record RequestRoadsidePurchaseCommand(int SlotIndex, [property: System.Text.Json.Serialization.JsonPropertyName("ItemGlobalId")] int ItemGlobalId) : CodecCommand<RequestRoadsidePurchaseCommand>, ICommandCodecProvider<RequestRoadsidePurchaseCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestRoadsidePurchaseCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.SlotIndex,
            Codecs.VarInt,
            static command => command.ItemGlobalId,
            static (slotIndex, itemGlobalId) => new RequestRoadsidePurchaseCommand(slotIndex, itemGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
