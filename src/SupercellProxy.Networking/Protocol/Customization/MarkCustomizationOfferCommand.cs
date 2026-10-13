using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Customization;

/// <summary>Marks the renovator stand's offer of one customization (native offer state 2).</summary>
/// <param name="CustomizationGlobalId">The customization data id.</param>
public sealed record MarkCustomizationOfferCommand(int CustomizationGlobalId) : CodecCommand<MarkCustomizationOfferCommand>, ICommandCodecProvider<MarkCustomizationOfferCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkCustomizationOfferCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.CustomizationGlobalId,
            static (customizationGlobalId) => new MarkCustomizationOfferCommand(customizationGlobalId)
        )
    );
}
