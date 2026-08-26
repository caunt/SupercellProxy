using System.Buffers.Binary;
using System.Globalization;
using System.IO.Compression;
using System.Text.Json;

namespace SupercellProxy.Playground.Json;

/// <summary>
/// Represents <c>CompressedJson</c>.
/// </summary>
public static class CompressedJson
{
    /// <summary>
    /// Executes the <c>Deserialize</c> operation.
    /// </summary>
    public static T Deserialize<T>(ReadOnlyMemory<byte> data)
    {
        var json = Decompress(data);
        return JsonSerializer.Deserialize<T>(json)
            ?? throw new InvalidDataException(
                $"Compressed JSON did not contain a {typeof(T).Name} value."
            );
    }

    /// <summary>
    /// Executes the <c>Decompress</c> operation.
    /// </summary>
    public static byte[] Decompress(ReadOnlyMemory<byte> data)
    {
        if (data.Length < sizeof(int))
            throw new InvalidDataException("Compressed JSON has no declared decompressed length.");

        var decompressedLength = BinaryPrimitives.ReadInt32LittleEndian(data.Span);

        if (decompressedLength < 0)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Invalid compressed JSON decompressed length: {decompressedLength}."
                )
            );

        using var input = new MemoryStream(data[sizeof(int)..].ToArray(), writable: false);
        using var zlib = new ZLibStream(input, CompressionMode.Decompress);
        var json = new byte[decompressedLength];
        zlib.ReadExactly(json);

        if (zlib.ReadByte() is not -1)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Compressed JSON exceeds its declared decompressed length of {decompressedLength} bytes."
                )
            );

        return json;
    }
}
