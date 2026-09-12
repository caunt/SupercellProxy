using System.Buffers.Binary;
using System.Globalization;

using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Proxy;

/// <summary>
/// Represents <c language="csharp">ProxyCaptureWriter</c>.
/// </summary>
public sealed class ProxyCaptureWriter
{

    private readonly string? _directoryPath;
    private long _sequence;

    /// <summary>
    /// Initializes a new <see cref="ProxyCaptureWriter"/> instance.
    /// </summary>
    public ProxyCaptureWriter(string? rootDirectoryPath, string remoteEndPoint, TimeProvider? timeProvider = null)
    {
        if (rootDirectoryPath is null)
            return;

        ArgumentException.ThrowIfNullOrWhiteSpace(rootDirectoryPath);

        string connectionName = string.Create(
            CultureInfo.InvariantCulture,
            $"{(timeProvider ?? TimeProvider.System).GetUtcNow():yyyyMMddTHHmmss.fffffffZ}-{SanitizeFileName(remoteEndPoint)}-{Guid.NewGuid():N}"
        );

        _directoryPath = Path.GetFullPath(Path.Combine(rootDirectoryPath, connectionName));
        new DirectoryInfo(_directoryPath).Create();
        TrySetDirectoryPermissions(_directoryPath);
    }
    /// <summary>
    /// Gets the <c language="csharp">DirectoryPath</c> value.
    /// </summary>
    public string DirectoryPath => _directoryPath ?? string.Empty;

    /// <summary>
    /// Provides the Get Direction Name value or operation.
    /// </summary>
    public static string GetDirectionName(MessageDirection direction)
    {
        return direction switch
        {
            MessageDirection.Clientbound => "clientbound",
            MessageDirection.Serverbound => "serverbound",
            _ => throw new ArgumentOutOfRangeException(nameof(direction)),
        };
    }

    /// <summary>
    /// Executes the <c language="csharp">SaveAsync</c> operation.
    /// </summary>
    public async ValueTask SaveAsync(
        string stage,
        MessageDirection direction,
        MessageContainer container,
        string messageName,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(container);

        if (_directoryPath is null)
            return;

        long sequence = Interlocked.Increment(ref _sequence);
        byte[] payload = container.Payload.ToArray();
        byte[] frame = new byte[7 + payload.Length];

        BinaryPrimitives.WriteUInt16BigEndian(frame.AsSpan(start: 0, length: 2), container.Identifier);
        frame[2] = byte.CreateTruncating(payload.Length >> 16);
        frame[3] = byte.CreateTruncating(payload.Length >> 8);
        frame[4] = byte.CreateTruncating(payload.Length);
        BinaryPrimitives.WriteUInt16BigEndian(frame.AsSpan(start: 5, length: 2), container.Version);
        payload.CopyTo(frame, index: 7);

        string fileName = string.Create(
            CultureInfo.InvariantCulture,
            $"{sequence:D8}-{stage}-{GetDirectionName(direction)}-{container.Identifier}-{container.Version}-{messageName}.bin"
        );

        string filePath = Path.Combine(_directoryPath, fileName);

        FileStream file = new(
            filePath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.Read,
            bufferSize: 4096,
            FileOptions.Asynchronous | FileOptions.WriteThrough
        );

        await using (file.ConfigureAwait(continueOnCapturedContext: false))
        {
            TrySetFilePermissions(filePath);
            await file.WriteAsync(frame, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    private static string SanitizeFileName(string value)
    {
        char[] invalidCharacters = Path.GetInvalidFileNameChars();

        return new string([.. value.Select(character => invalidCharacters.Contains(character) ? '_' : character)]);
    }

    private static void TrySetDirectoryPermissions(string path)
    {
        if (OperatingSystem.IsWindows())
            return;

        File.SetUnixFileMode(path, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute);
    }

    private static void TrySetFilePermissions(string path)
    {
        if (OperatingSystem.IsWindows())
            return;

        File.SetUnixFileMode(path, UnixFileMode.UserRead | UnixFileMode.UserWrite);
    }
}
