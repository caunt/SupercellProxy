using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>
/// Activates a booster held in the player's booster storage.
/// </summary>
/// <param name="BoosterDataGlobalId">The activated booster's data row in the boosters table.</param>
/// <param name="DiamondCost">The quoted activation price, zero for a free active slot.</param>
/// <param name="IsFree">Matches the stored booster slot's IsFree flag.</param>
/// <param name="Unknown2">Retained native modifier; only false is proven.</param>
public sealed record ActivateBoosterCommand(int BoosterDataGlobalId, int DiamondCost, bool IsFree, bool Unknown2) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ActivateBoosterCommandType;

    /// <summary>Decodes a booster activation command.</summary>
    public static ActivateBoosterCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int boosterDataGlobalId = stream.ReadVarInt();
        int diamondCost = stream.ReadVarInt();
        bool isFree = stream.ReadBoolean();
        bool unknown2 = stream.ReadBoolean();

        return new ActivateBoosterCommand(boosterDataGlobalId, diamondCost, isFree, unknown2);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(BoosterDataGlobalId);
        stream.WriteVarInt(DiamondCost);
        stream.WriteBoolean(IsFree);
        stream.WriteBoolean(Unknown2);
    }
}
