using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Consumables;

/// <summary>Uses a consumable on a game object.</summary>
/// <param name="TargetObjectGlobalId">Game object id the consumable acts on.</param>
/// <param name="ConsumableGlobalId">Global id of the consumable spent.</param>
public sealed record UseConsumableCommand(int TargetObjectGlobalId, int ConsumableGlobalId) : CodecCommand<UseConsumableCommand>, ICommandCodecProvider<UseConsumableCommand>
{
    /// <inheritdoc />
    public static CommandCodec<UseConsumableCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.TargetObjectGlobalId,
            Codecs.VarInt,
            static command => command.ConsumableGlobalId,
            static (targetObjectGlobalId, consumableGlobalId) => new UseConsumableCommand(targetObjectGlobalId, consumableGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
