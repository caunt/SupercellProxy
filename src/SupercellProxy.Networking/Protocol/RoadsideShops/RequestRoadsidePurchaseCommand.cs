using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Serverbound command buying a whole stand in the visited farm's roadside shop; the server answers with a
/// <see cref="BuyFromStandResponseMessage"/> and, on success, a <see cref="RoadsidePurchaseServerCommand"/>.
/// </summary>
/// <param name="SlotIndex">Position of the stand in the visited shop.</param>
/// <param name="ItemGlobalId">Global id of the goods the buyer expects on that stand.</param>
public sealed record RequestRoadsidePurchaseCommand(int SlotIndex, [property: JsonPropertyName("ItemGlobalId")] int ItemGlobalId) : CodecCommand<RequestRoadsidePurchaseCommand>, ICommandCodecProvider<RequestRoadsidePurchaseCommand>
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
