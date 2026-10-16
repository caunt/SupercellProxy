using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Removes an object from the edited layout.</summary>
/// <param name="ObjectGlobalId">Game object id of the object taken off the layout.</param>
/// <param name="PositionX">Horizontal position the object stood at.</param>
/// <param name="PositionY">Vertical position the object stood at.</param>
/// <param name="LayoutIndex">Layout slot being edited.</param>
public sealed record RemoveLayoutObjectCommand(int ObjectGlobalId, int PositionX, int PositionY, int LayoutIndex) : CodecCommand<RemoveLayoutObjectCommand>, ICommandCodecProvider<RemoveLayoutObjectCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RemoveLayoutObjectCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ObjectGlobalId,
            Codecs.VarInt,
            static command => command.PositionX,
            Codecs.VarInt,
            static command => command.PositionY,
            Codecs.VarInt,
            static command => command.LayoutIndex,
            static (objectGlobalId, positionX, positionY, layoutIndex) => new RemoveLayoutObjectCommand(objectGlobalId, positionX, positionY, layoutIndex)
        )
    );
}
