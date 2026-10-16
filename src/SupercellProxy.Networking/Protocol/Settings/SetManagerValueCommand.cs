using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings;

/// <summary>Stores a non-negative value on the avatar manager at native offset 0x1f0 when it changes.</summary>
/// <param name="Value">The stored value.</param>
public sealed record SetManagerValueCommand(int Value) : CodecCommand<SetManagerValueCommand>, ICommandCodecProvider<SetManagerValueCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetManagerValueCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.Value, static @value => new SetManagerValueCommand(value)));
}
