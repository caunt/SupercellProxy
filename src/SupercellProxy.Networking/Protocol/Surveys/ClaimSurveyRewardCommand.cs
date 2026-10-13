using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Surveys;

/// <summary>Claims the reward for a finished survey.</summary>
/// <param name="SurveyId">The survey id.</param>
/// <param name="RewardIndex">The reward slot.</param>
public sealed record ClaimSurveyRewardCommand(int SurveyId, int RewardIndex) : CodecCommand<ClaimSurveyRewardCommand>, ICommandCodecProvider<ClaimSurveyRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimSurveyRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.SurveyId,
            Codecs.VarInt,
            static command => command.RewardIndex,
            static (surveyId, rewardIndex) => new ClaimSurveyRewardCommand(surveyId, rewardIndex)
        ),
        CommandLayout.MetadataFirst
    );
}
