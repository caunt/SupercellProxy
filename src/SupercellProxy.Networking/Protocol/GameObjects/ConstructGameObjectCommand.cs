using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Serverbound command buying and placing a new object on the farm, such as a building, a field, a tree or a pet house, or
/// replacing an existing one with its next version.
/// </summary>
/// <param name="PositionX">Horizontal map position, aligned to the tile grid.</param>
/// <param name="Variant">Variant of the placed object; -1 for the default, which is the only value the ported paths accept.</param>
/// <param name="PositionY">Vertical map position, aligned to the tile grid.</param>
/// <param name="LayoutMode">Whether the object is placed from the farm layout editor; the ported paths accept only false.</param>
/// <param name="ReplacedObjectGlobalId">Global id of the object the new one replaces; -1 when placing a new object.</param>
/// <param name="TargetDataGlobalId">Global id of the table entry of the object to build.</param>
/// <param name="Mirrored">Whether the object is placed mirrored.</param>
public sealed record ConstructGameObjectCommand(
    int PositionX,
    int Variant,
    int PositionY,
    bool LayoutMode,
    [property: JsonPropertyName("ReplacedObjectGlobalId")] int ReplacedObjectGlobalId,
    [property: JsonPropertyName("TargetDataGlobalId")] int TargetDataGlobalId,
    bool Mirrored
)
    : CodecCommand<ConstructGameObjectCommand>, ICommandCodecProvider<ConstructGameObjectCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ConstructGameObjectCommand> Codec { get; } = Codecs.Command(
        Codecs.Versioned(
            Release17381.Version,
            Codecs.Composite(
                Codecs.Boolean,
                static command => command.LayoutMode,
                Codecs.VarInt,
                static command => command.Variant,
                Codecs.VarInt,
                static command => command.TargetDataGlobalId,
                Codecs.VarInt,
                static command => command.ReplacedObjectGlobalId,
                Codecs.VarInt,
                static command => command.PositionY,
                Codecs.VarInt,
                static command => command.PositionX,
                Codecs.Boolean,
                static command => command.Mirrored,
                static (layoutMode, variant, targetDataGlobalId, replacedObjectGlobalId, positionY, positionX, mirrored) => new ConstructGameObjectCommand(positionX, variant, positionY, layoutMode, replacedObjectGlobalId, targetDataGlobalId, mirrored)
            ),
            Codecs.Composite(
                Codecs.VarInt,
                static command => command.PositionX,
                Codecs.VarInt,
                static command => command.Variant,
                Codecs.VarInt,
                static command => command.PositionY,
                Codecs.Boolean,
                static command => command.LayoutMode,
                Codecs.VarInt,
                static command => command.ReplacedObjectGlobalId,
                Codecs.VarInt,
                static command => command.TargetDataGlobalId,
                Codecs.Boolean,
                static command => command.Mirrored,
                static (positionX, variant, positionY, layoutMode, replacedObjectGlobalId, targetDataGlobalId, mirrored) => new ConstructGameObjectCommand(positionX, variant, positionY, layoutMode, replacedObjectGlobalId, targetDataGlobalId, mirrored)
            )
        ),
        CommandLayout.MetadataFirst
    );
}
