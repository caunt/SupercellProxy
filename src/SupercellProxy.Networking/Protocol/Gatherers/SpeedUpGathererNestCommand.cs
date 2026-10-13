using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Gatherers;

/// <summary>Finishes a gatherer nest immediately for diamonds.</summary>
/// <param name="NestGlobalId">The gatherer nest game object id.</param>
public sealed record SpeedUpGathererNestCommand(int NestGlobalId) : CodecCommand<SpeedUpGathererNestCommand>, ICommandCodecProvider<SpeedUpGathererNestCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpGathererNestCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.NestGlobalId, static (nestGlobalId) => new SpeedUpGathererNestCommand(nestGlobalId))
    );
}
