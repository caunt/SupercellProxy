using System.Buffers.Binary;

namespace SupercellProxy.Networking.Json;

/// <summary>
/// Cheap shape check for the compressed JSON documents messages embed (a home's layout, for example), used while decoding to tell a
/// real document from bytes that belong to some other field.
/// </summary>
public static class CompressedDocumentValidator
{
    /// <summary>
    /// Checks the header without inflating anything: a non-negative little-endian length followed by a zlib stream header that
    /// uses deflate and passes zlib's header checksum.
    /// </summary>
    /// <param name="data">The embedded bytes; null or empty counts as a valid absent document.</param>
    /// <returns>False when the bytes cannot start a compressed document.</returns>
    public static bool IsValid(ReadOnlyMemory<byte>? data)
    {
        if (data is null || data.Value.IsEmpty)
            return true;

        ReadOnlySpan<byte> span = data.Value.Span;

        if (span.Length < 6 || BinaryPrimitives.ReadInt32LittleEndian(span) < 0)
            return false;

        ushort zlibHeader = BinaryPrimitives.ReadUInt16BigEndian(span[sizeof(int)..]);

        return (zlibHeader & 0x0F00) is 0x0800 && zlibHeader % 31 is 0;
    }
}
