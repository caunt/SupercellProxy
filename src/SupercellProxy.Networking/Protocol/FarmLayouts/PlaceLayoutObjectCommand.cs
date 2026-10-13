using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Places an object into the edited layout.</summary>
/// <param name="DataGlobalId">The object data id.</param>
/// <param name="VariantGlobalId">The variant data id.</param>
/// <param name="PositionX">The x coordinate.</param>
/// <param name="PositionY">The y coordinate.</param>
/// <param name="FromStorage">Whether the object comes from storage.</param>
/// <param name="Mirrored">Whether the object is mirrored.</param>
/// <param name="ObjectGlobalId">The object id.</param>
/// <param name="Rotation">The rotation value.</param>
/// <param name="LayoutIndex">The layout slot.</param>
public sealed record PlaceLayoutObjectCommand(
    int DataGlobalId,
    int VariantGlobalId,
    int PositionX,
    int PositionY,
    bool FromStorage,
    bool Mirrored,
    int ObjectGlobalId,
    int Rotation,
    int LayoutIndex
) : CodecCommand<PlaceLayoutObjectCommand>, ICommandCodecProvider<PlaceLayoutObjectCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PlaceLayoutObjectCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.DataGlobalId,
            Codecs.VarInt,
            static command => command.VariantGlobalId,
            Codecs.VarInt,
            static command => command.PositionX,
            Codecs.VarInt,
            static command => command.PositionY,
            Codecs.Boolean,
            static command => command.FromStorage,
            Codecs.Boolean,
            static command => command.Mirrored,
            Codecs.VarInt,
            static command => command.ObjectGlobalId,
            Codecs.VarInt,
            static command => command.Rotation,
            Codecs.VarInt,
            static command => command.LayoutIndex,
            static (dataGlobalId, variantGlobalId, positionX, positionY, fromStorage, mirrored, objectGlobalId, rotation, layoutIndex) => new PlaceLayoutObjectCommand(dataGlobalId, variantGlobalId, positionX, positionY, fromStorage, mirrored, objectGlobalId, rotation, layoutIndex)
        )
    );
}
