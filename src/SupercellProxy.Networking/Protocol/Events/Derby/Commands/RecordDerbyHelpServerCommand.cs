using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Reports successful help that advances the requesting player's derby task.</summary>
/// <param name="NeighborhoodId">The helper's neighbourhood, the first field.</param>
/// <param name="PlayerId">The avatar id of the player who gave the help.</param>
/// <param name="Status">The result code of the request, the last field; 1 marks success.</param>
public sealed record RecordDerbyHelpServerCommand(LongId NeighborhoodId, LongId PlayerId, int Status) : CodecServerCommand<RecordDerbyHelpServerCommand>, ICommandCodecProvider<RecordDerbyHelpServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RecordDerbyHelpServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.LongId,
        static command => command.NeighborhoodId,
        Codecs.LongId,
        static command => command.PlayerId,
        Codecs.VarInt,
        static command => command.Status,
        static (neighborhoodId, playerId, status) => new RecordDerbyHelpServerCommand(neighborhoodId, playerId, status)));
}
