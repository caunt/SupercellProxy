using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Achievements;

/// <summary>
/// Defines the Claim Achievement Reward Command contract.
/// </summary>
/// <summary>
/// Defines the Achievement Global Id contract.
/// </summary>
public sealed record ClaimAchievementRewardCommand([property: System.Text.Json.Serialization.JsonPropertyName("AchievementGlobalId")] int AchievementGlobalId) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.ClaimAchievementRewardCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static ClaimAchievementRewardCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int achievement = stream.ReadVarInt();

        return new ClaimAchievementRewardCommand(achievement);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(AchievementGlobalId);
    }
}
