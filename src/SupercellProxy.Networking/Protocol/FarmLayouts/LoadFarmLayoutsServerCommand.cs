using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>
/// Clientbound delivery of the player's saved farm layouts, so the client can show them in edit mode.
/// </summary>
/// <param name="GameMode">Game mode the layouts are for, as native numbers home modes.</param>
/// <param name="CompressedLayouts">The layouts as one compressed JSON document; null when there are none.</param>
public sealed record LoadFarmLayoutsServerCommand(int GameMode, ReadOnlyMemory<byte>? CompressedLayouts) : CodecServerCommand<LoadFarmLayoutsServerCommand>, ICommandCodecProvider<LoadFarmLayoutsServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<LoadFarmLayoutsServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.GameMode,
            Codecs.OptionalValue(Codecs.ByteArray),
            static command => command.CompressedLayouts,
            static (mode, layouts) => new LoadFarmLayoutsServerCommand(mode, layouts)
        ),
        CommandLayout.MetadataFirst
    );
}
