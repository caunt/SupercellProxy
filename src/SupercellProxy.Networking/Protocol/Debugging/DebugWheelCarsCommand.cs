using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Debugging;

/// <summary>Development-only command that acts on the prize wheel cars.</summary>
public sealed record DebugWheelCarsCommand : CodecCommand<DebugWheelCarsCommand>, ICommandCodecProvider<DebugWheelCarsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DebugWheelCarsCommand> Codec { get; } = Codecs.Command(type: 84, Codecs.Unit(static () => new DebugWheelCarsCommand()), CommandLayout.FieldsFirst, productionAllowed: false);
}
