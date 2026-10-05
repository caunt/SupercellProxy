using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Preserves the native ProcessBoatCrateHelpServerCommand wire contract.</summary>
/// <param name="HelperId">The HelperId value carried by the native command.</param>
/// <param name="HomeOwnerId">The HomeOwnerId value carried by the native command.</param>
/// <param name="ProductDataGlobalId">The ProductDataGlobalId value carried by the native command.</param>
/// <param name="Amount">The Amount value carried by the native command.</param>
/// <param name="CrateIndex">The CrateIndex value carried by the native command.</param>
/// <param name="BaseCoins">The BaseCoins value carried by the native command.</param>
/// <param name="BaseExperience">The BaseExperience value carried by the native command.</param>
/// <param name="DiamondCost">The DiamondCost value carried by the native command.</param>
/// <param name="BonusCoins">The BonusCoins value carried by the native command.</param>
/// <param name="BonusExperience">The BonusExperience value carried by the native command.</param>
/// <param name="RequestValue">The RequestValue value carried by the native command.</param>
/// <param name="UseDiamonds">The UseDiamonds value carried by the native command.</param>
public sealed record ProcessBoatCrateHelpServerCommand(
    LongId? HelperId,
    LongId? HomeOwnerId,
    int ProductDataGlobalId,
    int Amount,
    int CrateIndex,
    int BaseCoins,
    int BaseExperience,
    int DiamondCost,
    int BonusCoins,
    int BonusExperience,
    int RequestValue,
    bool UseDiamonds
) : ServerCommand
{
    /// <summary>Gets the native command type.</summary>
    public override int Type => CommandRegistry.ProcessBoatCrateHelpServerCommandType;

    /// <summary>Decodes the command-specific fields.</summary>
    public static ProcessBoatCrateHelpServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongId? helperId = stream.ReadOptionalLongId();
        LongId? homeOwnerId = stream.ReadOptionalLongId();
        int productDataGlobalId = stream.ReadVarInt();
        int amount = stream.ReadVarInt();
        int crateIndex = stream.ReadVarInt();
        int baseCoins = stream.ReadVarInt();
        int baseExperience = stream.ReadVarInt();
        int diamondCost = stream.ReadVarInt();
        int bonusCoins = stream.ReadVarInt();
        int bonusExperience = stream.ReadVarInt();
        int requestValue = stream.ReadVarInt();
        bool useDiamonds = stream.ReadBoolean();


        return new(
            helperId,
            homeOwnerId,
            productDataGlobalId,
            amount,
            crateIndex,
            baseCoins,
            baseExperience,
            diamondCost,
            bonusCoins,
            bonusExperience,
            requestValue,
            useDiamonds
        );
    }

    /// <summary>Encodes the unchanged native field order.</summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteOptionalLongId(HelperId);
        stream.WriteOptionalLongId(HomeOwnerId);
        stream.WriteVarInt(ProductDataGlobalId);
        stream.WriteVarInt(Amount);
        stream.WriteVarInt(CrateIndex);
        stream.WriteVarInt(BaseCoins);
        stream.WriteVarInt(BaseExperience);
        stream.WriteVarInt(DiamondCost);
        stream.WriteVarInt(BonusCoins);
        stream.WriteVarInt(BonusExperience);
        stream.WriteVarInt(RequestValue);
        stream.WriteBoolean(UseDiamonds);
    }
}
