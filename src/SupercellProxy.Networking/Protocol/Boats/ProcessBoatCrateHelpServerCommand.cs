using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

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
) : CodecServerCommand<ProcessBoatCrateHelpServerCommand>, ICommandCodecProvider<ProcessBoatCrateHelpServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ProcessBoatCrateHelpServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.OptionalLongId,
            static command => command.HelperId,
            Codecs.OptionalLongId,
            static command => command.HomeOwnerId,
            Codecs.VarInt,
            static command => command.ProductDataGlobalId,
            Codecs.VarInt,
            static command => command.Amount,
            Codecs.VarInt,
            static command => command.CrateIndex,
            Codecs.VarInt,
            static command => command.BaseCoins,
            Codecs.VarInt,
            static command => command.BaseExperience,
            Codecs.VarInt,
            static command => command.DiamondCost,
            Codecs.VarInt,
            static command => command.BonusCoins,
            Codecs.VarInt,
            static command => command.BonusExperience,
            Codecs.VarInt,
            static command => command.RequestValue,
            Codecs.Boolean,
            static command => command.UseDiamonds,
            static (
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
            ) => new ProcessBoatCrateHelpServerCommand(
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
            )
        )
    );
}
