using System.Buffers.Binary;
using System.Globalization;
using System.IO.Compression;
using System.Text.Json;

namespace SupercellProxy.Playground.Json;

/// <summary>
/// Represents <c language="csharp">CompressedJson</c>.
/// </summary>
internal static class CompressedJson
{
    /// <summary>
    /// Executes the <c language="csharp">Deserialize</c> operation.
    /// </summary>
    public static T Deserialize<T>(ReadOnlyMemory<byte> data)
    {
        var json = Decompress(data);
        TraceTaskEventPath(json);
        return JsonSerializer.Deserialize<T>(json)
            ?? throw new InvalidDataException(
                $"Compressed JSON did not contain a {typeof(T).Name} value."
            );
    }

    private static void TraceTaskEventPath(ReadOnlyMemory<byte> json)
    {
        if (
            !string.Equals(
                Environment.GetEnvironmentVariable("SUPERCELL_TRACE_TASK_EVENT_PATH"),
                "1",
                StringComparison.Ordinal
            )
        )
            return;

        using var document = JsonDocument.Parse(json);
        TraceTaskEventPath(document.RootElement, "$");
    }

    private static void TraceTaskEventPath(JsonElement element, string path)
    {
        if (element.ValueKind is JsonValueKind.Object)
        {
            var properties = element.EnumerateObject().ToArray();
            if (
                properties.Any(static property =>
                    string.Equals(property.Name, "seenInEventBoard", StringComparison.Ordinal)
                )
                || properties.Any(static property =>
                    string.Equals(property.Name, "id", StringComparison.Ordinal)
                )
                    && properties.Any(static property =>
                        string.Equals(property.Name, "variantId", StringComparison.Ordinal)
                    )
            )
                Console.Error.WriteLine(
                    $"Task-event JSON path: {path}; keys: {string.Join(',', properties.Select(static property => property.Name))}"
                );

            foreach (var property in properties)
                TraceTaskEventPath(property.Value, $"{path}.{property.Name}");
        }
        else if (element.ValueKind is JsonValueKind.Array)
        {
            foreach (var item in element.EnumerateArray())
                TraceTaskEventPath(item, path);
        }
    }

    /// <summary>
    /// Executes the <c language="csharp">Decompress</c> operation.
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
        using var zlib = new SynchronousZLibReader(input);
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

    private sealed class SynchronousZLibReader(Stream input) : IDisposable
    {
        private readonly ZLibStream _stream = new(input, CompressionMode.Decompress);

        public int ReadByte()
        {
            return _stream.ReadByte();
        }

        public void ReadExactly(byte[] buffer)
        {
            _stream.ReadExactly(buffer);
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}
