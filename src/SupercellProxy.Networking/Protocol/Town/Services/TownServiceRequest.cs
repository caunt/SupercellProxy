using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>One town service request entry.</summary>
/// <param name="ServiceBuildingGlobalId">The service building data id.</param>
/// <param name="Completed">The completed.</param>
/// <param name="Canceled">The canceled.</param>
/// <param name="RequiredGoods">The required goods.</param>
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
