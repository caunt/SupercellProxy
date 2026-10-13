using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Gatherers;

/// <summary>Buys a gatherer for a nest, placing it at the given position.</summary>
/// <param name="GathererGlobalId">The gatherer data id.</param>
/// <param name="PositionX">The placement x coordinate.</param>
/// <param name="PositionY">The placement y coordinate.</param>
/// <param name="NestObjectGlobalId">The nest object id.</param>
/// <param name="UseDiamonds">Whether the purchase is paid with diamonds.</param>
public sealed record BuyGathererCommand(int GathererGlobalId, int PositionX, int PositionY, int NestObjectGlobalId, bool UseDiamonds) : CodecCommand<BuyGathererCommand>, ICommandCodecProvider<BuyGathererCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyGathererCommand> Codec { get; } = Codecs.Command(
        type: 154,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.GathererGlobalId,
            Codecs.VarInt,
            static command => command.PositionX,
            Codecs.VarInt,
            static command => command.PositionY,
            Codecs.VarInt,
            static command => command.NestObjectGlobalId,
            Codecs.Boolean,
            static command => command.UseDiamonds,
            static (gathererGlobalId, positionX, positionY, nestObjectGlobalId, useDiamonds) => new BuyGathererCommand(gathererGlobalId, positionX, positionY, nestObjectGlobalId, useDiamonds)
        )
    );
}
