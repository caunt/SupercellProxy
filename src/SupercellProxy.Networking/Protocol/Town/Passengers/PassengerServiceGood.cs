using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Passengers;

/// <summary>One passenger service good entry.</summary>
/// <param name="GoodGlobalId">The good data id.</param>
/// <param name="Amount">The amount.</param>
public sealed record PassengerServiceGood(int GoodGlobalId, int Amount) : ICodecProvider<PassengerServiceGood>
{
    /// <inheritdoc />
    public static ICodec<PassengerServiceGood> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.GoodGlobalId,
        Codecs.VarInt,
        static value => value.Amount,
        static (goodGlobalId, amount) => new PassengerServiceGood(goodGlobalId, amount)
    );
}
