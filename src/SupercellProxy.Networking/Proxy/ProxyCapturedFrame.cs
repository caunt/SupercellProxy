using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Proxy;

/// <summary>Describes a captured protocol frame before it is persisted.</summary>
/// <param name="Sequence">Position of the frame in the connection's capture, starting at 1 and shared by both directions and stages.</param>
/// <param name="File">File name the frame is saved under: sequence, stage, direction, message id, version and message name, ending in <c>.bin</c>.</param>
/// <param name="Stage">Where in the pipeline the frame was taken: <c>incoming</c> as read from a peer, or <c>outgoing</c> as written after any rewriting.</param>
/// <param name="Direction">Whether the frame travels from the server to the client or the other way.</param>
/// <param name="Id">Message id from the frame header.</param>
/// <param name="Version">Message version from the frame header.</param>
/// <param name="Bytes">The whole plaintext frame: the 7-byte header (big-endian id, 24-bit length and version) followed by the decrypted payload.</param>
/// <param name="Timestamp">Value of the writer's clock, in its own ticks, taken when the frame reached the writer.</param>
/// <param name="TimestampFrequency">Ticks per second of that clock, for turning <paramref name="Timestamp"/> into elapsed time.</param>
public sealed record ProxyCapturedFrame(long Sequence,
    string File,
    string Stage,
    MessageDirection Direction,
    ushort Id,
    ushort Version,
    ReadOnlyMemory<byte> Bytes,
    long Timestamp,
    long TimestampFrequency);
