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
    /// <param name="data">A compressed document as messages embed it: a 4-byte little-endian count of inflated bytes, then a zlib stream that must inflate to exactly that many bytes.</param>
    /// <returns>The inflated UTF-8 JSON bytes; an empty array when the declared length is 0.</returns>
    /// <exception cref="InvalidDataException"><paramref name="data"/> is shorter than 4 bytes, declares a negative length, or inflates to more bytes than it declares.</exception>
    /// <remarks>A zlib stream that ends before the declared length, or is not valid zlib, fails with the <see cref="EndOfStreamException"/> or <see cref="InvalidDataException"/> raised by the decompressor.</remarks>
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
            ? throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Compressed JSON exceeds its declared decompressed length of {decompressedLength} bytes."))
            : document;
    }

    /// <summary>
    /// Executes the <c language="csharp">Deserialize</c> operation.
    /// </summary>
    /// <typeparam name="TValue">The System.Text.Json contract the inflated document is read into, such as the home or avatar save snapshot.</typeparam>
    /// <param name="data">The embedded document in the same form <see cref="Decompress"/> accepts: a 4-byte little-endian inflated length followed by a zlib stream.</param>
    /// <returns>The document deserialized as <typeparamref name="TValue"/>; it is never <see langword="null"/>.</returns>
    /// <exception cref="InvalidDataException">The data fails <see cref="Decompress"/>, or the JSON document is a bare <see langword="null"/> literal.</exception>
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
