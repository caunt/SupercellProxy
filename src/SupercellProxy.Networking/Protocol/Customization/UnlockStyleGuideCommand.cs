using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Customization;

/// <summary>Unlocks a customization style guide.</summary>
/// <param name="StyleGuideGlobalId">The style guide data id.</param>
public sealed record UnlockStyleGuideCommand(int StyleGuideGlobalId) : CodecCommand<UnlockStyleGuideCommand>, ICommandCodecProvider<UnlockStyleGuideCommand>
{
    /// <inheritdoc />
    public static CommandCodec<UnlockStyleGuideCommand> Codec { get; } = Codecs.Command(
        type: 238,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.StyleGuideGlobalId,
            static (styleGuideGlobalId) => new UnlockStyleGuideCommand(styleGuideGlobalId)
        )
    );
}
