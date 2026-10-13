using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Birthday;

/// <summary>Replaces the birthday event hug rewards.</summary>
/// <param name="Hugs">The hugs.</param>
public sealed record BirthdayHugsServerCommand(BirthdayHugs? Hugs) : CodecServerCommand<BirthdayHugsServerCommand>, ICommandCodecProvider<BirthdayHugsServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BirthdayHugsServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.Optional(BirthdayHugs.Codec), static command => command.Hugs, static (hugs) => new BirthdayHugsServerCommand(hugs))
    );
}
