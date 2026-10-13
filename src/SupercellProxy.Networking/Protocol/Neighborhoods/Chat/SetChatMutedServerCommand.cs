using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Sets whether the avatar is muted in chat.</summary>
/// <param name="Permanent">The permanent.</param>
/// <param name="Muted">The muted.</param>
public sealed record SetChatMutedServerCommand(bool Permanent, bool Muted) : CodecServerCommand<SetChatMutedServerCommand>, ICommandCodecProvider<SetChatMutedServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetChatMutedServerCommand> Codec { get; } = Codecs.Command(
        type: 839,
        Codecs.Composite(
            Codecs.Boolean,
            static command => command.Permanent,
            Codecs.Boolean,
            static command => command.Muted,
            static (permanent, muted) => new SetChatMutedServerCommand(permanent, muted)
        ),
        CommandLayout.MetadataFirst
    );
}
