using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Supplies the current player contribution and neighborhood derby progress.</summary>
/// <param name="PlayerPoints">The player's own derby points in the current derby.</param>
/// <param name="NeighborhoodPoints">The neighbourhood's total derby points in the current derby.</param>
/// <param name="BingoCount">The current bingo-derby counter the server reports.</param>
/// <param name="BingoValue">The current bingo-derby value the server reports; how it differs from <paramref name="BingoCount"/> is not yet identified.</param>
/// <param name="BunnyCount">The current bunny-derby counter the server reports.</param>
/// <param name="BingoLinesGained">Bingo lines completed since the last update.</param>
/// <param name="InstanceId">The derby instance the points belong to, or <see langword="null"/> when the server sends none.</param>
public sealed record UpdateDerbyPointsServerCommand(int PlayerPoints,
    int NeighborhoodPoints,
    int BingoCount,
    int BingoValue,
    int BunnyCount,
    int BingoLinesGained,
    LongId? InstanceId) : CodecServerCommand<UpdateDerbyPointsServerCommand>, ICommandCodecProvider<UpdateDerbyPointsServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<UpdateDerbyPointsServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
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
        static (playerPoints, neighborhoodPoints, bingoCount, bingoValue, bunnyCount, bingoLinesGained, instanceId) => new UpdateDerbyPointsServerCommand(playerPoints, neighborhoodPoints, bingoCount, bingoValue, bunnyCount, bingoLinesGained, instanceId)));
}
