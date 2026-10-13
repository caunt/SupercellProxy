using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>
/// Activates a booster held in the player's booster storage.
/// </summary>
/// <param name="BoosterDataGlobalId">The activated booster's data row in the boosters table.</param>
/// <param name="DiamondCost">The quoted activation price, zero for a free active slot.</param>
/// <param name="IsFree">Matches the stored booster slot's IsFree flag.</param>
/// <param name="Unknown2">Retained native modifier; only false is proven.</param>
public sealed record ActivateBoosterCommand(int BoosterDataGlobalId, int DiamondCost, bool IsFree, bool Unknown2) : CodecCommand<ActivateBoosterCommand>, ICommandCodecProvider<ActivateBoosterCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ActivateBoosterCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BoosterDataGlobalId,
            Codecs.VarInt,
            static command => command.DiamondCost,
            Codecs.Boolean,
            static command => command.IsFree,
            Codecs.Boolean,
            static command => command.Unknown2,
            static (boosterDataGlobalId, diamondCost, isFree, unknown2) => new ActivateBoosterCommand(boosterDataGlobalId, diamondCost, isFree, unknown2)
        )
    );
}
