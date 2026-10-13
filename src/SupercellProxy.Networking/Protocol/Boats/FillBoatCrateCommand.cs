using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Releases;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Fills a boat crate.</summary>
/// <param name="UseDirectRewards">Whether the crate rewards are granted directly.</param>
/// <param name="CrateIndex">The crate index.</param>
/// <param name="AllowCompletedOrder">Whether a completed order may still be filled.</param>
public sealed record FillBoatCrateCommand(bool UseDirectRewards, int CrateIndex, bool AllowCompletedOrder)
    : CodecCommand<FillBoatCrateCommand>, ICommandCodecProvider<FillBoatCrateCommand>
{
    /// <inheritdoc />
    public static CommandCodec<FillBoatCrateCommand> Codec { get; } = Codecs.Command(
        type: 634,
        Codecs.Versioned(
            Release17381.Version,
            Codecs.Composite(
                Codecs.Boolean,
                static command => command.UseDirectRewards,
                Codecs.Boolean,
                static command => command.AllowCompletedOrder,
                Codecs.VarInt,
                static command => command.CrateIndex,
                static (useDirectRewards, allowCompletedOrder, crateIndex) => new FillBoatCrateCommand(useDirectRewards, crateIndex, allowCompletedOrder)
            ),
            Codecs.Composite(
                Codecs.Boolean,
                static command => command.UseDirectRewards,
                Codecs.VarInt,
                static command => command.CrateIndex,
                Codecs.Boolean,
                static command => command.AllowCompletedOrder,
                static (useDirectRewards, crateIndex, allowCompletedOrder) => new FillBoatCrateCommand(useDirectRewards, crateIndex, allowCompletedOrder)
            )
        ),
        CommandLayout.MetadataFirst
    );
}
