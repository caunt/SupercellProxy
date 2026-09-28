using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>Buys one gift offered by the ordinary gift catalogue.</summary>
public sealed record BuyCatalogueGiftCommand(
    int GiftDataGlobalIdentifier,
    int CatalogueSlotDataGlobalIdentifier,
    int RewardSelectionValue,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.BuyCatalogueGiftCommandType;

    /// <summary>Decodes the native gift, catalogue-slot, and reward-selector fields.</summary>
    public static BuyCatalogueGiftCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int gift = stream.ReadVariableInt();
        int slot = stream.ReadVariableInt();
        int rewardSelection = stream.ReadVariableInt();
        (int phase, CommandData? debug0, CommandData? debug1) = DecodeCommand(stream, environment);

        return new BuyCatalogueGiftCommand(gift, slot, rewardSelection, phase, debug0, debug1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(GiftDataGlobalIdentifier);
        stream.WriteVariableInt(CatalogueSlotDataGlobalIdentifier);
        stream.WriteVariableInt(RewardSelectionValue);
        EncodeCommand(stream, environment);
    }
}
