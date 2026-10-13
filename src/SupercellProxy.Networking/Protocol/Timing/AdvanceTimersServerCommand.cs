using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Timing;

/// <summary>Advances the avatar timers by the given number of seconds.</summary>
/// <param name="Seconds">The number of seconds the timers advance.</param>
public sealed record AdvanceTimersServerCommand(int Seconds) : CodecServerCommand<AdvanceTimersServerCommand>, ICommandCodecProvider<AdvanceTimersServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AdvanceTimersServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.Seconds, static (seconds) => new AdvanceTimersServerCommand(seconds)));
}
