using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Achievements;

/// <summary>
/// Defines the Claim Achievement Reward Command contract.
/// </summary>
/// <summary>
/// Defines the Achievement Global Id contract.
/// </summary>
public sealed record ClaimAchievementRewardCommand([property: System.Text.Json.Serialization.JsonPropertyName("AchievementGlobalId")] int AchievementGlobalId) : CodecCommand<ClaimAchievementRewardCommand>, ICommandCodecProvider<ClaimAchievementRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimAchievementRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.AchievementGlobalId,
            static (achievement) => new ClaimAchievementRewardCommand(achievement)
        )
    );
}
