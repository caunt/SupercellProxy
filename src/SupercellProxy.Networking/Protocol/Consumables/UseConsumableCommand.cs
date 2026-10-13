using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Consumables;

/// <summary>Uses a consumable on a game object.</summary>
/// <param name="TargetObjectGlobalId">The target object id.</param>
/// <param name="ConsumableGlobalId">The consumable data id.</param>
public sealed record UseConsumableCommand(int TargetObjectGlobalId, int ConsumableGlobalId) : CodecCommand<UseConsumableCommand>, ICommandCodecProvider<UseConsumableCommand>
{
    /// <inheritdoc />
    public static CommandCodec<UseConsumableCommand> Codec { get; } = Codecs.Command(
        type: 546,
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
