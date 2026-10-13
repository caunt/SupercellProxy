using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Copies one edit-mode layout over another.</summary>
/// <param name="SourceSlotIndex">The copied layout slot index.</param>
/// <param name="TargetSlotIndex">The overwritten layout slot index.</param>
public sealed record CopyLayoutCommand(int SourceSlotIndex, int TargetSlotIndex) : CodecCommand<CopyLayoutCommand>, ICommandCodecProvider<CopyLayoutCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CopyLayoutCommand> Codec { get; } = Codecs.Command(
        type: 191,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.SourceSlotIndex,
            Codecs.VarInt,
            static command => command.TargetSlotIndex,
            static (sourceSlotIndex, targetSlotIndex) => new CopyLayoutCommand(sourceSlotIndex, targetSlotIndex)
        )
    );
}
