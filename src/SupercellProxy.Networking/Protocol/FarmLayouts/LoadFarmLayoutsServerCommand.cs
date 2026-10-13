using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>
/// Defines the Load Farm Layouts Server Command contract.
/// </summary>
/// <summary>
/// Defines the Game Mode contract.
/// </summary>
/// <summary>
/// Defines the Compressed Layouts contract.
/// </summary>
public sealed record LoadFarmLayoutsServerCommand(int GameMode, ReadOnlyMemory<byte>? CompressedLayouts) : CodecServerCommand<LoadFarmLayoutsServerCommand>, ICommandCodecProvider<LoadFarmLayoutsServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<LoadFarmLayoutsServerCommand> Codec { get; } = Codecs.Command(
        type: 743,
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
