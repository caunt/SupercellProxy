using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Passengers;

/// <summary>One passenger service entry.</summary>
/// <param name="ServiceBuildingGlobalId">The service building data id.</param>
/// <param name="Completed">The completed.</param>
/// <param name="Canceled">The canceled.</param>
/// <param name="RequiredGoods">The required goods.</param>
public sealed record PassengerService(int ServiceBuildingGlobalId, bool Completed, bool Canceled, ReadOnlyMemory<PassengerServiceGood> RequiredGoods) : ICodecProvider<PassengerService>
{
    /// <inheritdoc />
    public static ICodec<PassengerService> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.ServiceBuildingGlobalId,
        Codecs.Boolean,
        static value => value.Completed,
        Codecs.Boolean,
        static value => value.Canceled,
        Codecs.Memory(PassengerServiceGood.Codec),
        static value => value.RequiredGoods,
        static (serviceBuildingGlobalId, completed, canceled, requiredGoods) => new PassengerService(serviceBuildingGlobalId, completed, canceled, requiredGoods)
    );
}
