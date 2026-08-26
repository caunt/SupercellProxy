using System.Buffers.Binary;

namespace SupercellProxy.Playground.Network.Messages.Validation;

internal static class CompressedJsonPayloadValidator
{
    internal static bool IsValid(Memory<byte>? data)
    {
        if (data is null || data.Value.IsEmpty)
            return true;

        var span = data.Value.Span;
        if (span.Length < 6 || BinaryPrimitives.ReadInt32LittleEndian(span) < 0)
            return false;

        var zlibHeader = BinaryPrimitives.ReadUInt16BigEndian(span[sizeof(int)..]);
        return (zlibHeader & 0x0F00) is 0x0800 && zlibHeader % 31 is 0;
    }
}
