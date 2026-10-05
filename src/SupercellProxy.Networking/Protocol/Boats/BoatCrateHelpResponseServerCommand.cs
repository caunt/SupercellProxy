using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Preserves the native BoatCrateHelpResponseServerCommand wire contract.</summary>
/// <param name="HomeOwnerId">The HomeOwnerId value carried by the native command.</param>
/// <param name="HelperId">The HelperId value carried by the native command.</param>
/// <param name="CrateIndex">The CrateIndex value carried by the native command.</param>
/// <param name="ResponseCode">The ResponseCode value carried by the native command.</param>
/// <param name="ProductDataGlobalId">The ProductDataGlobalId value carried by the native command.</param>
/// <param name="Amount">The Amount value carried by the native command.</param>
/// <param name="BaseCoins">The BaseCoins value carried by the native command.</param>
/// <param name="BaseExperience">The BaseExperience value carried by the native command.</param>
/// <param name="BonusCoins">The BonusCoins value carried by the native command.</param>
/// <param name="BonusExperience">The BonusExperience value carried by the native command.</param>
/// <param name="DiamondCost">The DiamondCost value carried by the native command.</param>
/// <param name="RequestValue0">The RequestValue0 value carried by the native command.</param>
/// <param name="RequestValue1">The RequestValue1 value carried by the native command.</param>
public sealed record BoatCrateHelpResponseServerCommand(
    LongId? HomeOwnerId,
    LongId? HelperId,
    int CrateIndex,
    int ResponseCode,
    int ProductDataGlobalId,
    int Amount,
    int BaseCoins,
    int BaseExperience,
    int BonusCoins,
    int BonusExperience,
    int DiamondCost,
    int RequestValue0,
    int RequestValue1
) : ServerCommand
{
    /// <summary>Gets the native command type.</summary>
    public override int Type => CommandRegistry.BoatCrateHelpResponseServerCommandType;

    /// <summary>Decodes the command-specific fields.</summary>
    public static BoatCrateHelpResponseServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongId? homeOwnerId = stream.ReadOptionalLongId();
        LongId? helperId = stream.ReadOptionalLongId();
        int crateIndex = stream.ReadVarInt();
        int responseCode = stream.ReadVarInt();
        int productDataGlobalId = stream.ReadVarInt();
        int amount = stream.ReadVarInt();
        int baseCoins = stream.ReadVarInt();
        int baseExperience = stream.ReadVarInt();
        int bonusCoins = stream.ReadVarInt();
        int bonusExperience = stream.ReadVarInt();
        int diamondCost = stream.ReadVarInt();
        int requestValue0 = stream.ReadVarInt();
        int requestValue1 = stream.ReadVarInt();


        return new(
            homeOwnerId,
            helperId,
            crateIndex,
            responseCode,
            productDataGlobalId,
            amount,
            baseCoins,
            baseExperience,
            bonusCoins,
            bonusExperience,
            diamondCost,
            requestValue0,
            requestValue1
        );
    }

    /// <summary>Encodes the unchanged native field order.</summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteOptionalLongId(HomeOwnerId);
        stream.WriteOptionalLongId(HelperId);
        stream.WriteVarInt(CrateIndex);
        stream.WriteVarInt(ResponseCode);
        stream.WriteVarInt(ProductDataGlobalId);
        stream.WriteVarInt(Amount);
        stream.WriteVarInt(BaseCoins);
        stream.WriteVarInt(BaseExperience);
        stream.WriteVarInt(BonusCoins);
        stream.WriteVarInt(BonusExperience);
        stream.WriteVarInt(DiamondCost);
        stream.WriteVarInt(RequestValue0);
        stream.WriteVarInt(RequestValue1);
    }
}
