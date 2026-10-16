using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Counters;

/// <summary>Sets a native event counter.</summary>
/// <param name="Value">Number the counter is set to.</param>
/// <param name="CounterId">Which of native's event counters to set.</param>
public sealed record SetEventCounterCommand(int Value, int CounterId) : CodecCommand<SetEventCounterCommand>, ICommandCodecProvider<SetEventCounterCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetEventCounterCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Value,
            Codecs.VarInt,
            static command => command.CounterId,
            static (@value, counterId) => new SetEventCounterCommand(value, counterId)
        ),
        CommandLayout.MetadataFirst
    );
}
