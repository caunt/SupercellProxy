using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Achievements;

/// <summary>
/// Serverbound command claiming the reward of an achievement level the player has reached.
/// </summary>
/// <param name="AchievementGlobalId">Global id of the achievement in the achievements table.</param>
public sealed record ClaimAchievementRewardCommand([property: JsonPropertyName("AchievementGlobalId")] int AchievementGlobalId) : CodecCommand<ClaimAchievementRewardCommand>, ICommandCodecProvider<ClaimAchievementRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimAchievementRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.AchievementGlobalId,
            static achievement => new ClaimAchievementRewardCommand(achievement)
        )
    );
}
