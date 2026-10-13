using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Voting;

/// <summary>Delivers the next pair of decoration designs the avatar can vote on.</summary>
/// <param name="Pair">The candidate pair, or null when the server has none.</param>
public sealed record DecorationVoteCandidatesServerCommand(DecorationVotePair? Pair) : CodecServerCommand<DecorationVoteCandidatesServerCommand>, ICommandCodecProvider<DecorationVoteCandidatesServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DecorationVoteCandidatesServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Optional(DecorationVotePair.Codec),
            static command => command.Pair,
            static (pair) => new DecorationVoteCandidatesServerCommand(pair)
        ),
        CommandLayout.MetadataFirst
    );
}
