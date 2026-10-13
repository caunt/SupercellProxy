using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Replaces the remaining seconds of the active derby task timer.</summary>
public sealed record SetDerbyTaskTimeServerCommand(int RemainingSeconds) : CodecServerCommand<SetDerbyTaskTimeServerCommand>, ICommandCodecProvider<SetDerbyTaskTimeServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetDerbyTaskTimeServerCommand> Codec { get; } = Codecs.Command(
        type: 181,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.RemainingSeconds,
            static (remainingSeconds) => new SetDerbyTaskTimeServerCommand(remainingSeconds)
        )
    );
}
