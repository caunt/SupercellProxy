using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>Buys one gift offered by the ordinary gift catalogue.</summary>
public sealed record BuyCatalogueGiftCommand(int GiftDataGlobalId, int CatalogueSlotDataGlobalId, int RewardSelectionValue) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.BuyCatalogueGiftCommandType;

    /// <summary>Decodes the native gift, catalogue-slot, and reward-selector fields.</summary>
    public static BuyCatalogueGiftCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int gift = stream.ReadVarInt();
        int slot = stream.ReadVarInt();
        int rewardSelection = stream.ReadVarInt();


        return new BuyCatalogueGiftCommand(gift, slot, rewardSelection);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(GiftDataGlobalId);
        stream.WriteVarInt(CatalogueSlotDataGlobalId);
        stream.WriteVarInt(RewardSelectionValue);
    }
}
