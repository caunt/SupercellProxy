using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing.Molluscs;

/// <summary>Upgrades a mollusc bed and creates the molluscs of its new locations.</summary>
/// <param name="BedObjectGlobalId">The mollusc bed object id.</param>
public sealed record UpgradeMolluscBedCommand(int BedObjectGlobalId) : CodecCommand<UpgradeMolluscBedCommand>, ICommandCodecProvider<UpgradeMolluscBedCommand>
{
    /// <inheritdoc />
    public static CommandCodec<UpgradeMolluscBedCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BedObjectGlobalId,
            static bedObjectGlobalId => new UpgradeMolluscBedCommand(bedObjectGlobalId)
        )
    );
}
