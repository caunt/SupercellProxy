using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>Buys a booster with its native price.</summary>
/// <param name="BoosterGlobalId">The booster data id.</param>
/// <param name="Count">The number bought.</param>
public sealed record BuyBoosterCommand(int BoosterGlobalId, int Count) : CodecCommand<BuyBoosterCommand>, ICommandCodecProvider<BuyBoosterCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyBoosterCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BoosterGlobalId,
            Codecs.VarInt,
            static command => command.Count,
            static (boosterGlobalId, count) => new BuyBoosterCommand(boosterGlobalId, count)
        )
    );
}
