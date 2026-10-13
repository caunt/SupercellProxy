using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Supplies the current player contribution and neighborhood derby progress.</summary>
public sealed record UpdateDerbyPointsServerCommand(
    int PlayerPoints,
    int NeighborhoodPoints,
    int BingoCount,
    int BingoValue,
    int BunnyCount,
    int BingoLinesGained,
    LongId? InstanceId
) : CodecServerCommand<UpdateDerbyPointsServerCommand>, ICommandCodecProvider<UpdateDerbyPointsServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<UpdateDerbyPointsServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.PlayerPoints,
            Codecs.VarInt,
            static command => command.NeighborhoodPoints,
            Codecs.VarInt,
            static command => command.BingoCount,
            Codecs.VarInt,
            static command => command.BingoValue,
            Codecs.VarInt,
            static command => command.BunnyCount,
            Codecs.VarInt,
            static command => command.BingoLinesGained,
            Codecs.OptionalLongId,
            static command => command.InstanceId,
            static (playerPoints, neighborhoodPoints, bingoCount, bingoValue, bunnyCount, bingoLinesGained, instanceId) => new UpdateDerbyPointsServerCommand(playerPoints, neighborhoodPoints, bingoCount, bingoValue, bunnyCount, bingoLinesGained, instanceId)
        )
    );
}
