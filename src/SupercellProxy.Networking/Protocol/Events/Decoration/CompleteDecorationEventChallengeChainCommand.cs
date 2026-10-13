using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>Completes a decoration event challenge chain and grants its rewards.</summary>
/// <param name="ChallengeId">The challenge id.</param>
/// <param name="ChainIndex">The completed chain.</param>
public sealed record CompleteDecorationEventChallengeChainCommand(int ChallengeId, int ChainIndex) : CodecCommand<CompleteDecorationEventChallengeChainCommand>, ICommandCodecProvider<CompleteDecorationEventChallengeChainCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CompleteDecorationEventChallengeChainCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Int32,
            static command => command.ChallengeId,
            Codecs.Int32,
            static command => command.ChainIndex,
            static (challengeId, chainIndex) => new CompleteDecorationEventChallengeChainCommand(challengeId, chainIndex)
        ),
        CommandLayout.MetadataFirst
    );
}
