using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Inventory;

/// <summary>One granted resource entry.</summary>
/// <param name="ResourceGlobalId">The resource data id.</param>
/// <param name="Amount">The amount.</param>
public sealed record GrantedResource(int ResourceGlobalId, int Amount) : ICodecProvider<GrantedResource>
{
    /// <inheritdoc />
    public static ICodec<GrantedResource> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.ResourceGlobalId,
        Codecs.VarInt,
        static value => value.Amount,
        static (resourceGlobalId, amount) => new GrantedResource(resourceGlobalId, amount)
    );
}
