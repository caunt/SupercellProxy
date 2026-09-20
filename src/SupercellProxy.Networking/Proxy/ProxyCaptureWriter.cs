using System.Buffers.Binary;
using System.Globalization;
using System.Text;

using Nito.AsyncEx;

using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Proxy;

/// <summary>
/// Represents <c language="csharp">ProxyCaptureWriter</c>.
/// </summary>
public sealed class ProxyCaptureWriter
{
    private const long MaximumPendingBytes = 64L * 1024 * 1024;

    private readonly string? _directoryPath;
    private readonly AsyncLock _storageLock = new();
    private readonly List<PendingFrame> _pendingFrames = [];
    private readonly Dictionary<ArtifactName, byte[]> _pendingArtifacts = [];
    private readonly TimeProvider _timeProvider;
    private bool _abandoned;
    private long _pendingBytes;
    private int _persisted;
    private int _preserveForwardedFrames;
    private bool _publicationApproved;
    private long _sequence;

    /// <summary>
    /// Initializes a new <see cref="ProxyCaptureWriter"/> instance.
    /// </summary>
    public ProxyCaptureWriter(string? rootDirectoryPath, string remoteEndPoint, TimeProvider? timeProvider = null)
    {
        _timeProvider = timeProvider ?? TimeProvider.System;

        if (rootDirectoryPath is null)
            return;

        ArgumentException.ThrowIfNullOrWhiteSpace(rootDirectoryPath);

        string connectionName = string.Create(
            CultureInfo.InvariantCulture,
            $"{(timeProvider ?? TimeProvider.System).GetUtcNow():yyyyMMddTHHmmss.fffffffZ}-{SanitizeFileName(remoteEndPoint)}-{Guid.NewGuid():N}"
        );

        _directoryPath = Path.GetFullPath(Path.Combine(rootDirectoryPath, connectionName));
    }

    /// <summary>Optionally attaches metadata to each captured frame.</summary>
    public Func<ProxyCapturedFrame, ProxyCaptureAnnotation?>? AnnotateFrame { get; set; }

    /// <summary>Whether capture publication waits for an explicit call to <see cref="PublishAsync"/>.</summary>
    public bool DeferPublication { get; set; }

    /// <summary>
    /// Gets the <c language="csharp">DirectoryPath</c> value.
    /// </summary>
    public string DirectoryPath => _directoryPath ?? string.Empty;

    /// <summary>Whether this capture has begun writing to disk.</summary>
    public bool IsPersisted => Volatile.Read(ref _persisted) != 0;

    /// <summary>Whether the proxy should preserve received frame bytes while forwarding.</summary>
    public bool PreserveForwardedFrames
    {
        get => Volatile.Read(ref _preserveForwardedFrames) != 0;
        set => Volatile.Write(ref _preserveForwardedFrames, value ? 1 : 0);
    }

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

    /// <summary>Releases an unpublished attempt without touching the filesystem.</summary>
    public async Task DiscardPendingAsync()
    {
        using IDisposable gate = await _storageLock.LockAsync().ConfigureAwait(continueOnCapturedContext: false);

        _pendingFrames.Clear();
        _pendingArtifacts.Clear();
        _pendingBytes = 0;

        if (!IsPersisted)
            _abandoned = true;
    }

    /// <summary>Approves publication once at least one captured frame is available.</summary>
    public async Task PublishAsync(CancellationToken cancellationToken = default)
    {
        using IDisposable gate = await _storageLock.LockAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        _publicationApproved = true;
        await PublishPendingCoreAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    /// <summary>Stages an artifact without creating a capture directory before publication.</summary>
    public async Task SaveArtifactAsync(string directoryName, string name, ReadOnlyMemory<byte> contents, CancellationToken cancellationToken = default)
    {
        if (_directoryPath is null)
            throw new InvalidOperationException(message: "Artifacts require a capture path.");

        ValidateArtifactName(directoryName, name);

        using IDisposable gate = await _storageLock.LockAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        ArtifactName artifact = new(directoryName, name);

        if (IsPersisted)
        {
            await WriteArtifactAsync(artifact, contents, overwrite: true, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            return;
        }

        int previous = _pendingArtifacts.TryGetValue(artifact, out byte[]? existing) ? existing.Length : 0;
        RequirePendingCapacity(contents.Length - previous);
        _pendingArtifacts[artifact] = contents.ToArray();
        _pendingBytes += contents.Length - previous;
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

        long timestamp = _timeProvider.GetTimestamp();

        using IDisposable gate = await _storageLock.LockAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        long sequence = ++_sequence;
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

        ProxyCaptureAnnotation? annotation = AnnotateFrame?.Invoke(
            new ProxyCapturedFrame(
                sequence,
                fileName,
                stage,
                direction,
                container.Identifier,
                container.Version,
                frame,
                timestamp,
                _timeProvider.TimestampFrequency
            )
        );

        if (annotation is not null)
            ValidateArtifactName(annotation.Directory, annotation.File);

        PendingFrame pending = new(fileName, frame, annotation);

        if (IsPersisted)
        {
            await WriteFrameAsync(pending, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
        else
        {
            long size = frame.Length + (annotation is null ? 0 : Encoding.UTF8.GetByteCount(annotation.Text));
            RequirePendingCapacity(size);
            _pendingFrames.Add(pending);
            _pendingBytes += size;
            await PublishPendingCoreAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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

    private static void ValidateArtifactName(string directoryName, string name)
    {
        bool invalid = string.IsNullOrWhiteSpace(directoryName) || Path.GetFileName(directoryName) != directoryName || directoryName is "." or ".."
            || string.IsNullOrWhiteSpace(name) || Path.GetFileName(name) != name || name is "." or "..";

        if (invalid)
            throw new ArgumentException(message: "Artifact directories and names must be simple path segments.");
    }

    private static async Task WriteFileAsync(string path, ReadOnlyMemory<byte> bytes, bool overwrite, CancellationToken cancellationToken)
    {
        FileStream stream = new(
            path,
            overwrite ? FileMode.Create : FileMode.CreateNew,
            FileAccess.Write,
            FileShare.Read,
            bufferSize: 4096,
            FileOptions.Asynchronous | FileOptions.WriteThrough
        );

        await using (stream.ConfigureAwait(continueOnCapturedContext: false))
        {
            TrySetFilePermissions(path);
            await stream.WriteAsync(bytes, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    private async Task PublishPendingCoreAsync(CancellationToken cancellationToken)
    {
        if (_abandoned || IsPersisted || _pendingFrames.Count == 0 || (DeferPublication && !_publicationApproved))
            return;

        string path = _directoryPath ?? throw new InvalidOperationException(message: "Capture persistence is disabled.");
        DirectoryInfo directory = Directory.CreateDirectory(path);
        TrySetDirectoryPermissions(directory.FullName);
        Volatile.Write(ref _persisted, value: 1);

        foreach ((ArtifactName artifact, byte[] bytes) in _pendingArtifacts)
            await WriteArtifactAsync(artifact, bytes, overwrite: false, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        foreach (PendingFrame frame in _pendingFrames)
            await WriteFrameAsync(frame, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        _pendingFrames.Clear();
        _pendingArtifacts.Clear();
        _pendingBytes = 0;
    }

    private void RequirePendingCapacity(long additional)
    {
        if (_abandoned || _pendingBytes + additional > MaximumPendingBytes)
        {
            _abandoned = true;

            throw new IOException(message: "Capture admission buffer exceeded 64 MiB before publication; no messages were dropped.");
        }
    }

    private async Task WriteArtifactAsync(ArtifactName artifact, ReadOnlyMemory<byte> bytes, bool overwrite, CancellationToken cancellationToken)
    {
        string path = _directoryPath ?? throw new InvalidOperationException(message: "Capture persistence is disabled.");
        DirectoryInfo directory = Directory.CreateDirectory(Path.Combine(path, artifact.Directory));
        TrySetDirectoryPermissions(directory.FullName);
        await WriteFileAsync(Path.Combine(directory.FullName, artifact.File), bytes, overwrite, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    private async Task WriteFrameAsync(PendingFrame frame, CancellationToken cancellationToken)
    {
        string path = _directoryPath ?? throw new InvalidOperationException(message: "Capture persistence is disabled.");
        await WriteFileAsync(Path.Combine(path, frame.Name), frame.Bytes, overwrite: false, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if (frame.Annotation is not null)
        {
            DirectoryInfo directory = Directory.CreateDirectory(Path.Combine(path, frame.Annotation.Directory));
            TrySetDirectoryPermissions(directory.FullName);
            string index = Path.Combine(directory.FullName, frame.Annotation.File);
            await File.AppendAllTextAsync(index, frame.Annotation.Text, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            TrySetFilePermissions(index);
        }
    }

    private readonly record struct ArtifactName(string Directory, string File);
    private sealed record PendingFrame(string Name, byte[] Bytes, ProxyCaptureAnnotation? Annotation);
}
