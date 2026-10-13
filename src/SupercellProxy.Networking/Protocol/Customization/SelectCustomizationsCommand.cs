using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Customization;

/// <summary>Selects customizations for an object at the renovator stand (native LogicSelectCustomizationsCommand).</summary>
/// <param name="CustomizationGlobalId">The customization data id.</param>
/// <param name="OptionGlobalIds">The bought option data ids.</param>
/// <param name="ObjectGlobalId">The customized object id.</param>
public sealed record SelectCustomizationsCommand(int CustomizationGlobalId, ReadOnlyMemory<int> OptionGlobalIds, int ObjectGlobalId) : CodecCommand<SelectCustomizationsCommand>, ICommandCodecProvider<SelectCustomizationsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SelectCustomizationsCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.CustomizationGlobalId,
            Codecs.Memory(Codecs.VarInt),
            static command => command.OptionGlobalIds,
            Codecs.VarInt,
            static command => command.ObjectGlobalId,
            static (customizationGlobalId, optionGlobalIds, objectGlobalId) => new SelectCustomizationsCommand(customizationGlobalId, optionGlobalIds, objectGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
