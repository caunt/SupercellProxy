using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Inventory;

/// <summary>One resource the server grants the player.</summary>
/// <param name="ResourceGlobalId">Global id of the resource, such as goods or a currency.</param>
/// <param name="Amount">How much is granted.</param>
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
