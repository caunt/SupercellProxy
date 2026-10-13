using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.SupercellId;

/// <summary>Reports the result of logging the Supercell ID out of all devices.</summary>
/// <param name="ResultCode">The result code; zero is success.</param>
public sealed record SupercellIdLogoutAllDevicesResultMessage(int ResultCode) : CodecMessage<SupercellIdLogoutAllDevicesResultMessage>, ICodecProvider<SupercellIdLogoutAllDevicesResultMessage>
{
    /// <inheritdoc />
    public static ICodec<SupercellIdLogoutAllDevicesResultMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.ResultCode,
        static (resultCode) => new SupercellIdLogoutAllDevicesResultMessage(resultCode)
    );
}
