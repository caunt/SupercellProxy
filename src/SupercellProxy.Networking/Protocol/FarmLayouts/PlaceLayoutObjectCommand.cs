using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Places an object into the edited layout.</summary>
/// <param name="DataGlobalId">Global id of the object's type, such as a decoration entry.</param>
/// <param name="VariantGlobalId">Global id of the look variant the object uses.</param>
/// <param name="PositionX">Horizontal position on the farm.</param>
/// <param name="PositionY">Vertical position on the farm.</param>
/// <param name="FromStorage">Whether the object comes from storage.</param>
/// <param name="Mirrored">Whether the object is mirrored.</param>
/// <param name="ObjectGlobalId">Game object id of the object being placed.</param>
/// <param name="Rotation">Turned orientation of the object, as native numbers it.</param>
/// <param name="LayoutIndex">Layout slot being edited.</param>
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
