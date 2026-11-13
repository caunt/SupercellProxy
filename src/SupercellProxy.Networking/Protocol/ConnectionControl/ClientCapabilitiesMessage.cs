using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>Reports the client's device capabilities.</summary>
/// <param name="DeviceModel">Hardware model name of the device.</param>
/// <param name="Width">Screen width in pixels.</param>
/// <param name="Height">Screen height in pixels.</param>
/// <param name="MemoryMegabytes">Device memory size in megabytes.</param>
public sealed record ClientCapabilitiesMessage(string? DeviceModel, ushort Width, ushort Height, ushort MemoryMegabytes) : CodecMessage<ClientCapabilitiesMessage>, ICodecProvider<ClientCapabilitiesMessage>
{
    /// <inheritdoc />
    public static ICodec<ClientCapabilitiesMessage> Codec { get; } = Codecs.Composite(Codecs.OptionalUtf8String,
        static message => message.DeviceModel,
        Codecs.BigEndianUInt16,
        static message => message.Width,
        Codecs.BigEndianUInt16,
        static message => message.Height,
        Codecs.BigEndianUInt16,
        static message => message.MemoryMegabytes,
        static (deviceModel, width, height, memoryMegabytes) => new ClientCapabilitiesMessage(deviceModel, width, height, memoryMegabytes));
}
