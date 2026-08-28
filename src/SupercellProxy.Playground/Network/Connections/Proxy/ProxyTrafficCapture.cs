using System.Buffers.Binary;
using System.Globalization;
using SupercellProxy.Playground.Network.Messages;

namespace SupercellProxy.Playground.Network.Connections.Proxy;

/// <summary>
/// Represents <c language="csharp">ProxyTrafficCapture</c>.
/// </summary>
internal sealed class ProxyTrafficCapture
{
    /// Gets the default root directory for captured proxy traffic.
    public static string RootDirectoryPath { get; } =
        Path.Combine(AppContext.BaseDirectory, "proxy-captures");

    private readonly string _directoryPath;
    private long _sequence;

    /// <summary>
    /// Initializes a new <see cref="ProxyTrafficCapture"/> instance.
    /// </summary>
    public ProxyTrafficCapture(string rootDirectoryPath, string remoteEndPoint)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootDirectoryPath);

        var connectionName = string.Create(
            CultureInfo.InvariantCulture,
            $"{DateTime.UtcNow:yyyyMMddTHHmmss.fffffffZ}-{SanitizeFileName(remoteEndPoint)}-{Guid.NewGuid():N}"
        );
        _directoryPath = Path.GetFullPath(Path.Combine(rootDirectoryPath, connectionName));
        Directory.CreateDirectory(_directoryPath);
        TrySetDirectoryPermissions(_directoryPath);
    }

    /// <summary>
    /// Gets the <c language="csharp">DirectoryPath</c> value.
    /// </summary>
    public string DirectoryPath => _directoryPath;

    /// <summary>
    /// Executes the <c language="csharp">SaveAsync</c> operation.
    /// </summary>
    public async ValueTask SaveAsync(
        string stage,
        Direction direction,
        MessageContainer container,
        string messageName,
        CancellationToken cancellationToken = default
    )
    {
        var sequence = Interlocked.Increment(ref _sequence);
        var payload = container.Payload.ToArray();
        var frame = new byte[7 + payload.Length];

        BinaryPrimitives.WriteUInt16BigEndian(frame.AsSpan(0, 2), container.Id);
        frame[2] = byte.CreateTruncating((payload.Length >> 16));
        frame[3] = byte.CreateTruncating((payload.Length >> 8));
        frame[4] = byte.CreateTruncating(payload.Length);
        BinaryPrimitives.WriteUInt16BigEndian(frame.AsSpan(5, 2), container.Version);
        payload.CopyTo(frame, 7);

        var fileName = string.Create(
            CultureInfo.InvariantCulture,
            $"{sequence:D8}-{stage}-{GetDirectionName(direction)}-{container.Id}-{container.Version}-{messageName}.bin"
        );
        var filePath = Path.Combine(_directoryPath, fileName);

        var file = new FileStream(
            filePath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.Read,
            bufferSize: 4096,
            FileOptions.Asynchronous | FileOptions.WriteThrough
        );
        await using (file.ConfigureAwait(false))
        {
            TrySetFilePermissions(filePath);
            await file.WriteAsync(frame, cancellationToken).ConfigureAwait(false);
        }
    }

    internal static string GetDirectionName(Direction direction) =>
        direction switch
        {
            Direction.Clientbound => "clientbound",
            Direction.Serverbound => "serverbound",
            _ => throw new ArgumentOutOfRangeException(nameof(direction)),
        };

    private static string SanitizeFileName(string value)
    {
        var invalidCharacters = Path.GetInvalidFileNameChars();
        return new string(
            value
                .Select(character => invalidCharacters.Contains(character) ? '_' : character)
                .ToArray()
        );
    }

    private static void TrySetDirectoryPermissions(string path)
    {
        if (OperatingSystem.IsWindows())
            return;

        File.SetUnixFileMode(
            path,
            UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute
        );
    }

    private static void TrySetFilePermissions(string path)
    {
        if (OperatingSystem.IsWindows())
            return;

        File.SetUnixFileMode(path, UnixFileMode.UserRead | UnixFileMode.UserWrite);
    }
}
