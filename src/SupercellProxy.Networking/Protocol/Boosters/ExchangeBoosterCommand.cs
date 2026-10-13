using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>
/// Exchanges one booster held in the player's booster storage for another booster.
/// The native purpose of the trailing modifier field is unestablished; every retained input uses
/// <c>false</c>.
/// </summary>
/// <param name="RemovedBoosterDataGlobalId">The consumed booster's data row in the boosters table.</param>
/// <param name="AddedBoosterDataGlobalId">The granted booster's data row in the boosters table.</param>
/// <param name="Unknown0">Retained native modifier; only false is proven.</param>
public sealed record ExchangeBoosterCommand(int RemovedBoosterDataGlobalId, int AddedBoosterDataGlobalId, bool Unknown0) : CodecCommand<ExchangeBoosterCommand>, ICommandCodecProvider<ExchangeBoosterCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ExchangeBoosterCommand> Codec { get; } = Codecs.Command(
        type: 214,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.RemovedBoosterDataGlobalId,
            Codecs.Int32,
            static command => command.AddedBoosterDataGlobalId,
            Codecs.Boolean,
            static command => command.Unknown0,
            static (removedBoosterDataGlobalId, addedBoosterDataGlobalId, unknown0) => new ExchangeBoosterCommand(removedBoosterDataGlobalId, addedBoosterDataGlobalId, unknown0)
        )
    );
}
