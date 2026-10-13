using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Defines the Construct Game Object Command contract.
/// </summary>
/// <summary>
/// Defines the Position X contract.
/// </summary>
/// <summary>
/// Defines the Variant contract.
/// </summary>
/// <summary>
/// Defines the Position Y contract.
/// </summary>
/// <summary>
/// Defines the Layout Mode contract.
/// </summary>
/// <summary>
/// Defines the Replaced Object Global Id contract.
/// </summary>
/// <summary>
/// Defines the Target Data Global Id contract.
/// </summary>
/// <summary>
/// Defines the Mirrored contract.
/// </summary>
public sealed record ConstructGameObjectCommand(
    int PositionX,
    int Variant,
    int PositionY,
    bool LayoutMode,
    [property: System.Text.Json.Serialization.JsonPropertyName("ReplacedObjectGlobalId")] int ReplacedObjectGlobalId,
    [property: System.Text.Json.Serialization.JsonPropertyName("TargetDataGlobalId")] int TargetDataGlobalId,
    bool Mirrored
)
    : CodecCommand<ConstructGameObjectCommand>, ICommandCodecProvider<ConstructGameObjectCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ConstructGameObjectCommand> Codec { get; } = Codecs.Command(
        type: 577,
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
