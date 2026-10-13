using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing.Molluscs;

/// <summary>Finishes a growing mollusc immediately for diamonds.</summary>
/// <param name="MolluscGlobalId">The mollusc game object id.</param>
public sealed record SpeedUpMolluscCommand(int MolluscGlobalId) : CodecCommand<SpeedUpMolluscCommand>, ICommandCodecProvider<SpeedUpMolluscCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpMolluscCommand> Codec { get; } = Codecs.Command(
        type: 395,
        Codecs.Composite(Codecs.VarInt, static command => command.MolluscGlobalId, static (molluscGlobalId) => new SpeedUpMolluscCommand(molluscGlobalId))
    );
}
