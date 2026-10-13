using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Activates an edit-mode layout.</summary>
/// <param name="SlotIndex">The activated layout slot index.</param>
public sealed record SetActiveLayoutCommand(int SlotIndex) : CodecCommand<SetActiveLayoutCommand>, ICommandCodecProvider<SetActiveLayoutCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetActiveLayoutCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.SlotIndex, static (slotIndex) => new SetActiveLayoutCommand(slotIndex))
    );
}
