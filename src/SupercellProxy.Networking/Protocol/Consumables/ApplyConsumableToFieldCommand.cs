using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Consumables;

/// <summary>Applies a growth consumable to a field.</summary>
/// <param name="FieldObjectGlobalId">The field object id.</param>
/// <param name="ConsumableGlobalId">The consumable data id.</param>
public sealed record ApplyConsumableToFieldCommand(int FieldObjectGlobalId, int ConsumableGlobalId) : CodecCommand<ApplyConsumableToFieldCommand>, ICommandCodecProvider<ApplyConsumableToFieldCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ApplyConsumableToFieldCommand> Codec { get; } = Codecs.Command(
        type: 2626,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.FieldObjectGlobalId,
            Codecs.VarInt,
            static command => command.ConsumableGlobalId,
            static (fieldObjectGlobalId, consumableGlobalId) => new ApplyConsumableToFieldCommand(fieldObjectGlobalId, consumableGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
