using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Captures;

/// <summary>One line of a capture's packet index, describing a saved frame file.</summary>
/// <param name="Sequence">The frame's position in the capture.</param>
/// <param name="File">The frame's file name inside the capture directory.</param>
/// <param name="Stage">The pipeline stage at which the frame was saved.</param>
/// <param name="Direction">The frame direction name.</param>
/// <param name="Id">The message identifier.</param>
/// <param name="Version">The message version.</param>
/// <param name="Length">The payload length in bytes.</param>
/// <param name="Sha256">The uppercase hexadecimal SHA-256 of the payload.</param>
/// <param name="Timestamp">The monotonic timestamp at which the frame was observed.</param>
/// <param name="TimestampFrequency">The number of timestamp ticks per second.</param>
/// <param name="Turn">The native turn that produced the frame, when known.</param>
/// <param name="AssetFingerprint">The asset fingerprint active for the frame, when known.</param>
public sealed record CapturedPacket(
    long Sequence,
    string File,
    string Stage,
    string Direction,
    [property: JsonPropertyName("Identifier")] ushort Id,
    ushort Version,
    int Length,
    string Sha256,
    long Timestamp,
    long TimestampFrequency,
    CapturedTurn? Turn,
    string? AssetFingerprint
);
