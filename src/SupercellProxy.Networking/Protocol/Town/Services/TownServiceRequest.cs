using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>One service a passenger at a town service building wants.</summary>
/// <param name="ServiceBuildingGlobalId">Global id of the service building's table entry that provides the service.</param>
/// <param name="Completed">Whether the passenger already got this service.</param>
/// <param name="Canceled">Whether the service request was dropped without being done.</param>
/// <param name="RequiredGoods">Goods the building uses up to provide the service.</param>
public sealed record TownServiceRequest(int ServiceBuildingGlobalId, bool Completed, bool Canceled, ReadOnlyMemory<TownServiceGood> RequiredGoods) : ICodecProvider<TownServiceRequest>
{
    /// <inheritdoc />
    public static ICodec<TownServiceRequest> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.ServiceBuildingGlobalId,
        Codecs.Boolean,
        static value => value.Completed,
        Codecs.Boolean,
        static value => value.Canceled,
        Codecs.Memory(TownServiceGood.Codec),
        static value => value.RequiredGoods,
        static (serviceBuildingGlobalId, completed, canceled, requiredGoods) => new TownServiceRequest(serviceBuildingGlobalId, completed, canceled, requiredGoods)
    );
}
