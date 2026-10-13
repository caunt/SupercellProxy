using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Removes an object from the edited layout.</summary>
/// <param name="ObjectGlobalId">The object id.</param>
/// <param name="PositionX">The object x coordinate.</param>
/// <param name="PositionY">The object y coordinate.</param>
/// <param name="LayoutIndex">The layout slot.</param>
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
