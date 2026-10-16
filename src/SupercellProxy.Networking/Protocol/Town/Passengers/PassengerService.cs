using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Passengers;

/// <summary>One service a moved town passenger wants, carried between towns.</summary>
/// <param name="ServiceBuildingGlobalId">Global id of the service building's table entry that provides the service.</param>
/// <param name="Completed">Whether the passenger already got this service.</param>
/// <param name="Canceled">Whether the service request was dropped without being done.</param>
/// <param name="RequiredGoods">Goods the service building uses up to provide the service.</param>
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
