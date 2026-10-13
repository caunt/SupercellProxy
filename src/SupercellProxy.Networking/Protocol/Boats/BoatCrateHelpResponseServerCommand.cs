using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

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
) : CodecServerCommand<BoatCrateHelpResponseServerCommand>, ICommandCodecProvider<BoatCrateHelpResponseServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BoatCrateHelpResponseServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.OptionalLongId,
            static command => command.HomeOwnerId,
            Codecs.OptionalLongId,
            static command => command.HelperId,
            Codecs.VarInt,
            static command => command.CrateIndex,
            Codecs.VarInt,
            static command => command.ResponseCode,
            Codecs.VarInt,
            static command => command.ProductDataGlobalId,
            Codecs.VarInt,
            static command => command.Amount,
            Codecs.VarInt,
            static command => command.BaseCoins,
            Codecs.VarInt,
            static command => command.BaseExperience,
            Codecs.VarInt,
            static command => command.BonusCoins,
            Codecs.VarInt,
            static command => command.BonusExperience,
            Codecs.VarInt,
            static command => command.DiamondCost,
            Codecs.VarInt,
            static command => command.RequestValue0,
            Codecs.VarInt,
            static command => command.RequestValue1,
            static (
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
            ) => new BoatCrateHelpResponseServerCommand(
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
            )
        )
    );
}
