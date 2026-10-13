using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>Discards a booster awaiting a keep, swap, or discard decision.</summary>
public sealed record DiscardPendingBoosterCommand(int BoosterDataGlobalId) : CodecCommand<DiscardPendingBoosterCommand>, ICommandCodecProvider<DiscardPendingBoosterCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DiscardPendingBoosterCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BoosterDataGlobalId,
            static (boosterDataGlobalId) => new DiscardPendingBoosterCommand(boosterDataGlobalId)
        )
    );
}
