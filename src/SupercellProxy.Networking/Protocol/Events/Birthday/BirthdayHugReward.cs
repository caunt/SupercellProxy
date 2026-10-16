using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Birthday;

/// <summary>One birthday hug reward entry.</summary>
/// <param name="GoodGlobalId">Global id of the goods this reward gives.</param>
/// <param name="Amount">How many of those goods.</param>
public sealed record BirthdayHugReward(int GoodGlobalId, int Amount) : ICodecProvider<BirthdayHugReward>
{
    /// <inheritdoc />
    public static ICodec<BirthdayHugReward> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.GoodGlobalId,
        Codecs.VarInt,
        static value => value.Amount,
        static (goodGlobalId, amount) => new BirthdayHugReward(goodGlobalId, amount)
    );
}
