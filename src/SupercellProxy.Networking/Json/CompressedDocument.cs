using System.Buffers.Binary;
using System.Globalization;
using System.IO.Compression;
using System.Text.Json;

namespace SupercellProxy.Networking.Json;

/// <summary>
/// Represents <c language="csharp">CompressedJson</c>.
/// </summary>
public static class CompressedDocument
{

    /// <summary>
    /// Executes the <c language="csharp">Decompress</c> operation.
    /// </summary>
    public static byte[] Decompress(ReadOnlyMemory<byte> data)
    {
        if (data.Length < sizeof(int))
            throw new InvalidDataException(message: "Compressed JSON has no declared decompressed length.");

        int decompressedLength = BinaryPrimitives.ReadInt32LittleEndian(data.Span);

        if (decompressedLength < 0)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid compressed JSON decompressed length: {decompressedLength}."));

        using MemoryStream input = new(data[sizeof(int)..].ToArray(), writable: false);

        using SynchronousZLibReader zlib = new(input);

        byte[] document = new byte[decompressedLength];
        zlib.ReadExactly(document);

        return zlib.ReadByte() is not -1
            ? throw new InvalidDataException(
                string.Create(CultureInfo.InvariantCulture, $"Compressed JSON exceeds its declared decompressed length of {decompressedLength} bytes.")
            )
            : document;
    }

    /// <summary>
    /// Executes the <c language="csharp">Deserialize</c> operation.
    /// </summary>
    public static TValue Deserialize<TValue>(ReadOnlyMemory<byte> data)
    {
        byte[] document = Decompress(data);

        return JsonSerializer.Deserialize<TValue>(document)
            ?? throw new InvalidDataException($"Compressed JSON did not contain a {typeof(TValue).Name} value.");
    }

    private sealed class SynchronousZLibReader(Stream input) : IDisposable
    {
        private readonly ZLibStream _stream = new(input, CompressionMode.Decompress);

        public void Dispose()
        {
            _stream.Dispose();
        }

        public int ReadByte()
        {
            return _stream.ReadByte();
        }

        public void ReadExactly(byte[] buffer)
        {
            _stream.ReadExactly(buffer);
        }
    }
}
