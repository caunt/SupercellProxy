using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Customization;

/// <summary>Applies a customization to an object part at the renovator stand.</summary>
/// <param name="PartGlobalId">The part data id.</param>
/// <param name="CustomizationGlobalId">The customization data id.</param>
public sealed record ApplyCustomizationCommand(int PartGlobalId, int CustomizationGlobalId) : CodecCommand<ApplyCustomizationCommand>, ICommandCodecProvider<ApplyCustomizationCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ApplyCustomizationCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.PartGlobalId,
            Codecs.VarInt,
            static command => command.CustomizationGlobalId,
            static (partGlobalId, customizationGlobalId) => new ApplyCustomizationCommand(partGlobalId, customizationGlobalId)
        )
    );
}
