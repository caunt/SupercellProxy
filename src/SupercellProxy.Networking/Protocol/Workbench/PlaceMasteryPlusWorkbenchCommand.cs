using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Workbench;

/// <summary>Places the mastery-plus workbench.</summary>
/// <param name="PositionY">The position y.</param>
/// <param name="PositionX">The position x.</param>
public sealed record PlaceMasteryPlusWorkbenchCommand(int PositionY, int PositionX) : CodecCommand<PlaceMasteryPlusWorkbenchCommand>, ICommandCodecProvider<PlaceMasteryPlusWorkbenchCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PlaceMasteryPlusWorkbenchCommand> Codec { get; } = Codecs.Command(
        type: 658,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.PositionY,
            Codecs.VarInt,
            static command => command.PositionX,
            static (positionY, positionX) => new PlaceMasteryPlusWorkbenchCommand(positionY, positionX)
        ),
        CommandLayout.MetadataFirst
    );
}
