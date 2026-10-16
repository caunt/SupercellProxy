using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Passengers;

/// <summary>One kind of goods a passenger's service uses up.</summary>
/// <param name="GoodGlobalId">Global id of the goods.</param>
/// <param name="Amount">How many of the goods the service uses up.</param>
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
