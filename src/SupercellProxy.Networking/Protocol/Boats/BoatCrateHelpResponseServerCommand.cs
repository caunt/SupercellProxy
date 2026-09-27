using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Preserves the native BoatCrateHelpResponseServerCommand wire contract.</summary>
/// <param name="HomeOwnerIdentifier">The HomeOwnerIdentifier value carried by the native command.</param>
/// <param name="HelperIdentifier">The HelperIdentifier value carried by the native command.</param>
/// <param name="CrateIndex">The CrateIndex value carried by the native command.</param>
/// <param name="ResponseCode">The ResponseCode value carried by the native command.</param>
/// <param name="ProductDataGlobalIdentifier">The ProductDataGlobalIdentifier value carried by the native command.</param>
/// <param name="Amount">The Amount value carried by the native command.</param>
/// <param name="BaseCoins">The BaseCoins value carried by the native command.</param>
/// <param name="BaseExperience">The BaseExperience value carried by the native command.</param>
/// <param name="BonusCoins">The BonusCoins value carried by the native command.</param>
/// <param name="BonusExperience">The BonusExperience value carried by the native command.</param>
/// <param name="DiamondCost">The DiamondCost value carried by the native command.</param>
/// <param name="RequestValue0">The RequestValue0 value carried by the native command.</param>
/// <param name="RequestValue1">The RequestValue1 value carried by the native command.</param>
/// <param name="ServerCommandIdentifier"></param>
/// <param name="ExecutionPhaseCounter"></param>
/// <param name="DebugData0"></param>
/// <param name="DebugData1"></param>
public sealed record BoatCrateHelpResponseServerCommand(
    LongIdentifier? HomeOwnerIdentifier,
    LongIdentifier? HelperIdentifier,
    int CrateIndex,
    int ResponseCode,
    int ProductDataGlobalIdentifier,
    int Amount,
    int BaseCoins,
    int BaseExperience,
    int BonusCoins,
    int BonusExperience,
    int DiamondCost,
    int RequestValue0,
    int RequestValue1,
    int ServerCommandIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : ServerCommand(ServerCommandIdentifier, ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>Gets the native command type.</summary>
    public override int Type => CommandRegistry.BoatCrateHelpResponseServerCommandType;

    /// <summary>Decodes the body followed by its command header.</summary>
    public static BoatCrateHelpResponseServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongIdentifier? homeOwnerIdentifier = stream.ReadOptionalLongIdentifier();
        LongIdentifier? helperIdentifier = stream.ReadOptionalLongIdentifier();
        int crateIndex = stream.ReadVariableInt();
        int responseCode = stream.ReadVariableInt();
        int productDataGlobalIdentifier = stream.ReadVariableInt();
        int amount = stream.ReadVariableInt();
        int baseCoins = stream.ReadVariableInt();
        int baseExperience = stream.ReadVariableInt();
        int bonusCoins = stream.ReadVariableInt();
        int bonusExperience = stream.ReadVariableInt();
        int diamondCost = stream.ReadVariableInt();
        int requestValue0 = stream.ReadVariableInt();
        int requestValue1 = stream.ReadVariableInt();
        (int identifier, (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields) = DecodeServerCommand(stream, environment);

        return new(
            homeOwnerIdentifier,
            helperIdentifier,
            crateIndex,
            responseCode,
            productDataGlobalIdentifier,
            amount,
            baseCoins,
            baseExperience,
            bonusCoins,
            bonusExperience,
            diamondCost,
            requestValue0,
            requestValue1,
            identifier,
            fields.ExecutionPhaseCounter,
            fields.DebugData0,
            fields.DebugData1
        );
    }

    /// <summary>Encodes the unchanged native field order.</summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteOptionalLongIdentifier(HomeOwnerIdentifier);
        stream.WriteOptionalLongIdentifier(HelperIdentifier);
        stream.WriteVariableInt(CrateIndex);
        stream.WriteVariableInt(ResponseCode);
        stream.WriteVariableInt(ProductDataGlobalIdentifier);
        stream.WriteVariableInt(Amount);
        stream.WriteVariableInt(BaseCoins);
        stream.WriteVariableInt(BaseExperience);
        stream.WriteVariableInt(BonusCoins);
        stream.WriteVariableInt(BonusExperience);
        stream.WriteVariableInt(DiamondCost);
        stream.WriteVariableInt(RequestValue0);
        stream.WriteVariableInt(RequestValue1);
        EncodeServerCommand(stream, environment);
    }
}
