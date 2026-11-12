using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing.Molluscs;

/// <summary>Gives Angus one stored mollusc to open for meat and possible bonus pearls.</summary>
/// <param name="MolluscDataId">Global id of the mollusc goods row to open, written first as a VarInt.</param>
/// <param name="AngusId">Game object global id of the Angus character who opens it, written as a VarInt after the mollusc id.</param>
public sealed record OpenMolluscCommand(int MolluscDataId, int AngusId) : CodecCommand<OpenMolluscCommand>, ICommandCodecProvider<OpenMolluscCommand>
{
    /// <inheritdoc />
    public static CommandCodec<OpenMolluscCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
            static command => command.MolluscDataId,
            Codecs.VarInt,
            static command => command.AngusId,
            static (molluscDataId, angusId) => new OpenMolluscCommand(molluscDataId, angusId)),
        CommandLayout.MetadataFirst);
}
