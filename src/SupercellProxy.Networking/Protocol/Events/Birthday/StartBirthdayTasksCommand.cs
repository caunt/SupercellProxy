using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Birthday;

/// <summary>Starts the birthday event task list.</summary>
public sealed record StartBirthdayTasksCommand : CodecCommand<StartBirthdayTasksCommand>, ICommandCodecProvider<StartBirthdayTasksCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartBirthdayTasksCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new StartBirthdayTasksCommand()));
}
