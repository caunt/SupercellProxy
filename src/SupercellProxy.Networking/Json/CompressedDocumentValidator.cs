using System.Buffers.Binary;

namespace SupercellProxy.Networking.Json;

/// <summary>
/// Defines the Compressed Json Payload Validator contract.
/// </summary>
public static class CompressedDocumentValidator
{
    /// <summary>
    /// Provides the Is Valid value or operation.
    /// </summary>
    public static bool IsValid(Memory<byte>? data)
    {
        if (data is null || data.Value.IsEmpty)
            return true;

        Span<byte> span = data.Value.Span;

        if (span.Length < 6 || BinaryPrimitives.ReadInt32LittleEndian(span) < 0)
            return false;

        ushort zlibHeader = BinaryPrimitives.ReadUInt16BigEndian(span[sizeof(int)..]);

        return (zlibHeader & 0x0F00) is 0x0800 && zlibHeader % 31 is 0;
    }
}
