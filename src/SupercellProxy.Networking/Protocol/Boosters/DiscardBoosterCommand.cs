using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>Discards an owned booster.</summary>
/// <param name="BoosterGlobalId">The booster data id.</param>
/// <param name="Active">Whether the discarded booster is the active one.</param>
/// <param name="Silent">Whether the discard skips the notification.</param>
public sealed record DiscardBoosterCommand(int BoosterGlobalId, bool Active, bool Silent) : CodecCommand<DiscardBoosterCommand>, ICommandCodecProvider<DiscardBoosterCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DiscardBoosterCommand> Codec { get; } = Codecs.Command(
        type: 213,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BoosterGlobalId,
            Codecs.Boolean,
            static command => command.Active,
            Codecs.Boolean,
            static command => command.Silent,
            static (boosterGlobalId, active, silent) => new DiscardBoosterCommand(boosterGlobalId, active, silent)
        )
    );
}
