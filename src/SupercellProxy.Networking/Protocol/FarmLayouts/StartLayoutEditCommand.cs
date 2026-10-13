using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Starts editing an edit-mode layout.</summary>
/// <param name="SlotIndex">The edited layout slot index.</param>
public sealed record StartLayoutEditCommand(int SlotIndex) : CodecCommand<StartLayoutEditCommand>, ICommandCodecProvider<StartLayoutEditCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartLayoutEditCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.SlotIndex, static (slotIndex) => new StartLayoutEditCommand(slotIndex))
    );
}
