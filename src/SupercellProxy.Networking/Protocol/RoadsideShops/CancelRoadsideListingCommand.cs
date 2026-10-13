using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>Cancels an unsold listing; native configuration determines item return and diamond cost.</summary>
public sealed record CancelRoadsideListingCommand(int SlotIndex) : CodecCommand<CancelRoadsideListingCommand>, ICommandCodecProvider<CancelRoadsideListingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CancelRoadsideListingCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.SlotIndex, static (slotIndex) => new CancelRoadsideListingCommand(slotIndex)),
        CommandLayout.MetadataFirst
    );
}
