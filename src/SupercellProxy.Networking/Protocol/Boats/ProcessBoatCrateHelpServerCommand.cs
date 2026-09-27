using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Preserves the native ProcessBoatCrateHelpServerCommand wire contract.</summary>
/// <param name="HelperIdentifier">The HelperIdentifier value carried by the native command.</param>
/// <param name="HomeOwnerIdentifier">The HomeOwnerIdentifier value carried by the native command.</param>
/// <param name="ProductDataGlobalIdentifier">The ProductDataGlobalIdentifier value carried by the native command.</param>
/// <param name="Amount">The Amount value carried by the native command.</param>
/// <param name="CrateIndex">The CrateIndex value carried by the native command.</param>
/// <param name="BaseCoins">The BaseCoins value carried by the native command.</param>
/// <param name="BaseExperience">The BaseExperience value carried by the native command.</param>
/// <param name="DiamondCost">The DiamondCost value carried by the native command.</param>
/// <param name="BonusCoins">The BonusCoins value carried by the native command.</param>
/// <param name="BonusExperience">The BonusExperience value carried by the native command.</param>
/// <param name="RequestValue">The RequestValue value carried by the native command.</param>
/// <param name="UseDiamonds">The UseDiamonds value carried by the native command.</param>
/// <param name="ServerCommandIdentifier"></param>
/// <param name="ExecutionPhaseCounter"></param>
/// <param name="DebugData0"></param>
/// <param name="DebugData1"></param>
public sealed record ProcessBoatCrateHelpServerCommand(
    LongIdentifier? HelperIdentifier,
    LongIdentifier? HomeOwnerIdentifier,
    int ProductDataGlobalIdentifier,
    int Amount,
    int CrateIndex,
    int BaseCoins,
    int BaseExperience,
    int DiamondCost,
    int BonusCoins,
    int BonusExperience,
    int RequestValue,
    bool UseDiamonds,
    int ServerCommandIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : ServerCommand(ServerCommandIdentifier, ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>Gets the native command type.</summary>
    public override int Type => CommandRegistry.ProcessBoatCrateHelpServerCommandType;

    /// <summary>Decodes the body followed by its command header.</summary>
    public static ProcessBoatCrateHelpServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongIdentifier? helperIdentifier = stream.ReadOptionalLongIdentifier();
        LongIdentifier? homeOwnerIdentifier = stream.ReadOptionalLongIdentifier();
        int productDataGlobalIdentifier = stream.ReadVariableInt();
        int amount = stream.ReadVariableInt();
        int crateIndex = stream.ReadVariableInt();
        int baseCoins = stream.ReadVariableInt();
        int baseExperience = stream.ReadVariableInt();
        int diamondCost = stream.ReadVariableInt();
        int bonusCoins = stream.ReadVariableInt();
        int bonusExperience = stream.ReadVariableInt();
        int requestValue = stream.ReadVariableInt();
        bool useDiamonds = stream.ReadBoolean();
        (int identifier, (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields) = DecodeServerCommand(stream, environment);

        return new(
            helperIdentifier,
            homeOwnerIdentifier,
            productDataGlobalIdentifier,
            amount,
            crateIndex,
            baseCoins,
            baseExperience,
            diamondCost,
            bonusCoins,
            bonusExperience,
            requestValue,
            useDiamonds,
            identifier,
            fields.ExecutionPhaseCounter,
            fields.DebugData0,
            fields.DebugData1
        );
    }

    /// <summary>Encodes the unchanged native field order.</summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteOptionalLongIdentifier(HelperIdentifier);
        stream.WriteOptionalLongIdentifier(HomeOwnerIdentifier);
        stream.WriteVariableInt(ProductDataGlobalIdentifier);
        stream.WriteVariableInt(Amount);
        stream.WriteVariableInt(CrateIndex);
        stream.WriteVariableInt(BaseCoins);
        stream.WriteVariableInt(BaseExperience);
        stream.WriteVariableInt(DiamondCost);
        stream.WriteVariableInt(BonusCoins);
        stream.WriteVariableInt(BonusExperience);
        stream.WriteVariableInt(RequestValue);
        stream.WriteBoolean(UseDiamonds);
        EncodeServerCommand(stream, environment);
    }
}
