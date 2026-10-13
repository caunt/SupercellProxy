using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Sanctuary.Puzzles;

/// <summary>Removes a puzzle piece the server revoked.</summary>
/// <param name="PuzzlePieceGlobalId">The puzzle piece data id.</param>
public sealed record RevokePuzzlePieceServerCommand(int PuzzlePieceGlobalId) : CodecServerCommand<RevokePuzzlePieceServerCommand>, ICommandCodecProvider<RevokePuzzlePieceServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RevokePuzzlePieceServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.PuzzlePieceGlobalId,
            static (puzzlePieceGlobalId) => new RevokePuzzlePieceServerCommand(puzzlePieceGlobalId)
        )
    );
}
