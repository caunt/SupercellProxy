using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using SupercellProxy.Playground.Commands;

namespace SupercellProxy.Playground.Network.Transport;

/// <summary>
/// Initializes a new <see cref="MessageStream"/> instance.
/// </summary>
public partial class MessageStream(Stream stream, bool leaveOpen = true)
    : IAsyncDisposable,
        IDisposable
{
    private readonly Stream stream = stream;
    private readonly bool leaveOpen = leaveOpen;

    /// <summary>
    /// Defines the <c>MaxPayloadLength</c> value.
    /// </summary>
    public const int MaxPayloadLength = 0x1000000;

    /// <summary>
    /// Gets or sets the <c>Position</c> value.
    /// </summary>
    public long Position
    {
        get => GetMemoryStream().Position;
        set => GetMemoryStream().Position = value;
    }

    /// <summary>
    /// Gets the <c>Length</c> value.
    /// </summary>
    public long Length => GetMemoryStream().Length;

    /// <summary>
    /// Gets or sets the <c>CommandDataResolver</c> value.
    /// </summary>
    public ICommandDataResolver? CommandDataResolver { get; set; }

    /// <summary>
    /// Creates a <c>MessageStream</c> from the supplied data.
    /// </summary>
    public static MessageStream Create()
    {
        return new MessageStream(new MemoryStream());
    }

    /// <summary>
    /// Executes the <c>ToArray</c> operation.
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
        memoryStream = stream as MemoryStream;
        return memoryStream is not null;
    }

    private static MessageStream CreateOfflineStream(ReadOnlyMemory<byte> memory)
    {
        if (MemoryMarshal.TryGetArray(memory, out var segment) && segment.Array is not null)
            return new MessageStream(
                new MemoryStream(segment.Array, segment.Offset, segment.Count)
            );

        return new MessageStream(new MemoryStream(memory.ToArray()));
    }

    /// <summary>
    /// Executes the <c>DisposeAsync</c> operation.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        FlushWriteBoolean();

        if (leaveOpen)
            return;

        await stream.DisposeAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the <c>Dispose</c> operation.
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);

        FlushWriteBoolean();

        if (leaveOpen)
            return;

        stream.Dispose();
    }

    /// <summary>
    /// Executes the <c>ToString</c> operation.
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
