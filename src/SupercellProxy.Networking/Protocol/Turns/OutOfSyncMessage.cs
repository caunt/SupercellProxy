using System.Globalization;

using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Turns;

/// <summary>
/// <para>OutOfSyncMessage (23626) wire representation.</para>
/// </summary>
public sealed record OutOfSyncMessage : IMessage
{

    /// <summary>
    /// Gets or sets the <c language="csharp">ClientChecksum</c> value.
    /// </summary>
    public string? ClientChecksum { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ClientState</c> value.
    /// </summary>
    public string? ClientState { get; init; }
    /// <summary>
    /// Gets or sets the <c language="csharp">HasDiagnostics</c> value.
    /// </summary>
    public bool HasDiagnostics { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ServerChecksum</c> value.
    /// </summary>
    public string? ServerChecksum { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ServerState</c> value.
    /// </summary>
    public string? ServerState { get; init; }

    /// <summary>
    /// Creates a <c language="csharp">OutOfSyncMessage</c> from the supplied data.
    /// </summary>
    public static OutOfSyncMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;

        OutOfSyncMessage message = new()
        {
            HasDiagnostics = stream.ReadBoolean(),
            ServerChecksum = stream.ReadOptionalString(),
            ClientChecksum = stream.ReadOptionalString(),
            ServerState = stream.ReadOptionalString(),
            ClientState = stream.ReadOptionalString(),
        };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unexpected trailing {nameof(OutOfSyncMessage)} data at position {stream.Position} of {stream.Length}."
                )
            )
            : message;
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteBoolean(HasDiagnostics);
        stream.WriteOptionalString(ServerChecksum);
        stream.WriteOptionalString(ClientChecksum);
        stream.WriteOptionalString(ServerState);
        stream.WriteOptionalString(ClientState);

        return new MessageContainer(identifier, version, stream);
    }
}
