using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Birthday;

/// <summary>The rewards the birthday event's hug exchange holds for the player.</summary>
/// <param name="Rewards">Goods waiting for the player from hugs, one entry per kind of goods.</param>
/// <param name="Total">Hug total the server reports alongside the rewards.</param>
public sealed record BirthdayHugs(ReadOnlyMemory<BirthdayHugReward> Rewards, int Total) : ICodecProvider<BirthdayHugs>
{
    /// <inheritdoc />
    public static ICodec<BirthdayHugs> Codec { get; } = Codecs.Composite(
        Codecs.Memory(BirthdayHugReward.Codec),
        static value => value.Rewards,
        Codecs.VarInt,
        static value => value.Total,
        static (rewards, total) => new BirthdayHugs(rewards, total)
    );
}
