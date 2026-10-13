using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Billing;

/// <summary>Reports the result of claiming an item bought in the Supercell ID store.</summary>
/// <param name="ProductId">The claimed product id, checked against the pending claim.</param>
/// <param name="ClaimToken">The claim token, checked against the pending claim.</param>
/// <param name="ResultCode">The result code; zero is success.</param>
public sealed record SupercellIdClaimItemResultMessage(string? ProductId, string? ClaimToken, int ResultCode) : CodecMessage<SupercellIdClaimItemResultMessage>, ICodecProvider<SupercellIdClaimItemResultMessage>
{
    /// <inheritdoc />
    public static ICodec<SupercellIdClaimItemResultMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.ProductId,
        Codecs.OptionalString,
        static message => message.ClaimToken,
        Codecs.Int32,
        static message => message.ResultCode,
        static (productId, claimToken, resultCode) => new SupercellIdClaimItemResultMessage(productId, claimToken, resultCode)
    );
}
