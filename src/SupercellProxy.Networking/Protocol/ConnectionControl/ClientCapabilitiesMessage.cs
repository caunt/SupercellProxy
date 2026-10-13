using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>Reports the client's device capabilities.</summary>
/// <param name="DeviceModel">The device model.</param>
/// <param name="Width">The width.</param>
/// <param name="Height">The height.</param>
/// <param name="MemoryMegabytes">The memory megabytes.</param>
public sealed record ClientCapabilitiesMessage(string? DeviceModel, ushort Width, ushort Height, ushort MemoryMegabytes) : CodecMessage<ClientCapabilitiesMessage>, ICodecProvider<ClientCapabilitiesMessage>
{
    /// <inheritdoc />
    public static ICodec<ClientCapabilitiesMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.DeviceModel,
        Codecs.UInt16,
        static message => message.Width,
        Codecs.UInt16,
        static message => message.Height,
        Codecs.UInt16,
        static message => message.MemoryMegabytes,
        static (deviceModel, width, height, memoryMegabytes) => new ClientCapabilitiesMessage(deviceModel, width, height, memoryMegabytes)
    );
}
