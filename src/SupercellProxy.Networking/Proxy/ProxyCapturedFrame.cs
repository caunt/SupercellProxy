using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Proxy;

/// <summary>Describes a captured protocol frame before it is persisted.</summary>
public sealed record ProxyCapturedFrame(
    long Sequence,
    string File,
    string Stage,
    MessageDirection Direction,
    ushort Id,
    ushort Version,
    ReadOnlyMemory<byte> Bytes,
    long Timestamp,
    long TimestampFrequency
);
