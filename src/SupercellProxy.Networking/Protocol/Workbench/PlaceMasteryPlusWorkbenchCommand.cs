using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Workbench;

/// <summary>Places the mastery-plus workbench.</summary>
/// <param name="PositionY">Vertical farm position the workbench is placed at.</param>
/// <param name="PositionX">Horizontal farm position the workbench is placed at.</param>
public sealed record PlaceMasteryPlusWorkbenchCommand(int PositionY, int PositionX) : CodecCommand<PlaceMasteryPlusWorkbenchCommand>, ICommandCodecProvider<PlaceMasteryPlusWorkbenchCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PlaceMasteryPlusWorkbenchCommand> Codec { get; } = Codecs.Command(
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
