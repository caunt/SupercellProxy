using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace SupercellProxy.Playground.Network.Streams;

public partial class SupercellStream(Stream stream, bool leaveOpen = true) : IAsyncDisposable, IDisposable
{
    public const int MaxPayloadLength = 0x1000000;
    public long Position { get => GetMemoryStream().Position; set => GetMemoryStream().Position = value; }
    public long Length => GetMemoryStream().Length;

    public static SupercellStream Create()
    {
        return new SupercellStream(new MemoryStream());
    }

    public byte[] ToArray()
    {
        return GetMemoryStream().ToArray();
    }

    private MemoryStream GetMemoryStream()
    {
        if (!TryGetMemoryStream(out var memoryStream))
            throw new NotSupportedException("This is online stream.");

        return memoryStream;
    }

    private bool TryGetMemoryStream([MaybeNullWhen(false)] out MemoryStream memoryStream)
    {
        memoryStream = stream as MemoryStream;
        return memoryStream is not null;
    }

    private static SupercellStream CreateOfflineStream(ReadOnlyMemory<byte> memory)
    {
        if (MemoryMarshal.TryGetArray(memory, out var segment) && segment.Array is not null)
            return new SupercellStream(new MemoryStream(segment.Array, segment.Offset, segment.Count));

        return new SupercellStream(new MemoryStream(memory.ToArray()));
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        FlushWriteBoolean();

        if (leaveOpen)
            return;

        await stream.DisposeAsync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        FlushWriteBoolean();

        if (leaveOpen)
            return;

        stream.Dispose();
    }

    public override string? ToString()
    {
        if (!TryGetMemoryStream(out var memoryStream))
            return base.ToString();

        var builder = new StringBuilder();
        var hex = Convert.ToHexString(memoryStream.ToArray());

        if (hex.Length <= 128)
        {
            builder.Append(hex);
        }
        else
        {
            builder.Append(hex, 0, 125);
            builder.Append("...");
        }

        return builder.ToString();
    }
}
