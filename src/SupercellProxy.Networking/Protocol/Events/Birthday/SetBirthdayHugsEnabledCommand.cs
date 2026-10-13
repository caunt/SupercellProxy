using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Birthday;

/// <summary>Turns the birthday event hug exchange on or off.</summary>
/// <param name="Enabled">The enabled.</param>
public sealed record SetBirthdayHugsEnabledCommand(bool Enabled) : CodecCommand<SetBirthdayHugsEnabledCommand>, ICommandCodecProvider<SetBirthdayHugsEnabledCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetBirthdayHugsEnabledCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.Boolean, static command => command.Enabled, static (enabled) => new SetBirthdayHugsEnabledCommand(enabled))
    );
}
