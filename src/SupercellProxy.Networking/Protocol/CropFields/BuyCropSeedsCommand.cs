using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.CropFields;

/// <summary>Buys seeds of a crop.</summary>
/// <param name="CropGlobalId">The crop data id.</param>
public sealed record BuyCropSeedsCommand(int CropGlobalId) : CodecCommand<BuyCropSeedsCommand>, ICommandCodecProvider<BuyCropSeedsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyCropSeedsCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.CropGlobalId, static (cropGlobalId) => new BuyCropSeedsCommand(cropGlobalId)),
        CommandLayout.MetadataFirst
    );
}
