using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Sanctuary;

/// <summary>Places the received puzzle pieces of a sanctuary animal.</summary>
/// <param name="AnimalGlobalId">The sanctuary animal data id.</param>
/// <param name="ClearAllPending">Whether every pending puzzle selection is cleared, not only the current ones.</param>
public sealed record PlaceSanctuaryPuzzlePiecesCommand(int AnimalGlobalId, bool ClearAllPending) : CodecCommand<PlaceSanctuaryPuzzlePiecesCommand>, ICommandCodecProvider<PlaceSanctuaryPuzzlePiecesCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PlaceSanctuaryPuzzlePiecesCommand> Codec { get; } = Codecs.Command(
        type: 221,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.AnimalGlobalId,
            Codecs.Boolean,
            static command => command.ClearAllPending,
            static (animalGlobalId, clearAllPending) => new PlaceSanctuaryPuzzlePiecesCommand(animalGlobalId, clearAllPending)
        )
    );
}
