using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using SupercellProxy.Playground.Commands;

namespace SupercellProxy.Playground.Network.Transport;

/// <summary>
/// Initializes a new <see cref="MessageStream"/> instance.
/// </summary>
internal sealed partial class MessageStream(Stream stream, bool leaveOpen = true) : IDisposable
{
    private readonly Stream _stream = stream;
    private readonly bool _leaveOpen = leaveOpen;

    /// <summary>
    /// Defines the <c language="csharp">MaxPayloadLength</c> value.
    /// </summary>
    public const int MaxPayloadLength = 0x1000000;

    /// <summary>
    /// Gets or sets the <c language="csharp">Position</c> value.
    /// </summary>
    public long Position
    {
        get => GetMemoryStream().Position;
        set => GetMemoryStream().Position = value;
    }

    /// <summary>
    /// Gets the <c language="csharp">Length</c> value.
    /// </summary>
    public long Length => GetMemoryStream().Length;

    /// <summary>
    /// Gets or sets the <c language="csharp">CommandDataResolver</c> value.
    /// </summary>
    public ICommandDataResolver? CommandDataResolver { get; set; }

    /// <summary>
    /// Creates a <c language="csharp">MessageStream</c> from the supplied data.
    /// </summary>
    public static MessageStream Create()
    {
        return new MessageStream(new MemoryStream());
    }

    /// <summary>
    /// Executes the <c language="csharp">ToArray</c> operation.
    /// </summary>
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
        memoryStream = _stream as MemoryStream;
        return memoryStream is not null;
    }

    internal static MessageStream Create(ReadOnlyMemory<byte> memory)
    {
        if (MemoryMarshal.TryGetArray(memory, out var segment) && segment.Array is not null)
            return new MessageStream(
                new MemoryStream(segment.Array, segment.Offset, segment.Count)
            );

        return new MessageStream(new MemoryStream(memory.ToArray()));
    }

    /// <summary>
    /// Executes the <c language="csharp">Dispose</c> operation.
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);

        FlushWriteBoolean();

        if (_leaveOpen)
            return;

        _stream.Dispose();
    }

    /// <summary>
    /// Executes the <c language="csharp">ToString</c> operation.
    /// </summary>
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
