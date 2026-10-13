using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Birthday;

/// <summary>The birthday hugs.</summary>
/// <param name="Rewards">The rewards.</param>
/// <param name="Total">The total.</param>
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
