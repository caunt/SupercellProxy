using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>Advertises an existing stand using a free opportunity or a purchased advertisement credit.</summary>
public sealed record AdvertiseRoadsideListingCommand(int SlotIndex) : CodecCommand<AdvertiseRoadsideListingCommand>, ICommandCodecProvider<AdvertiseRoadsideListingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AdvertiseRoadsideListingCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.SlotIndex, static slotIndex => new AdvertiseRoadsideListingCommand(slotIndex)),
        CommandLayout.MetadataFirst
    );
}
