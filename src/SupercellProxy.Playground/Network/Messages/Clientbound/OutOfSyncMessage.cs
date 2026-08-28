using System.Globalization;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

/// <summary>
/// <para>OutOfSyncMessage (23626) wire representation.</para>
/// </summary>
internal sealed record OutOfSyncMessage : IMessage
{
    /// <summary>
    /// Gets or sets the <c language="csharp">HasDiagnostics</c> value.
    /// </summary>
    public bool HasDiagnostics { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ServerChecksum</c> value.
    /// </summary>
    public string? ServerChecksum { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ClientChecksum</c> value.
    /// </summary>
    public string? ClientChecksum { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ServerState</c> value.
    /// </summary>
    public string? ServerState { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ClientState</c> value.
    /// </summary>
    public string? ClientState { get; init; }

    /// <summary>
    /// Creates a <c language="csharp">OutOfSyncMessage</c> from the supplied data.
    /// </summary>
    public static OutOfSyncMessage Create(MessageContainer container)
    {
        var stream = container.Payload;
        var message = new OutOfSyncMessage
        {
            HasDiagnostics = stream.ReadBoolean(),
            ServerChecksum = stream.ReadOptionalString(),
            ClientChecksum = stream.ReadOptionalString(),
            ServerState = stream.ReadOptionalString(),
            ClientState = stream.ReadOptionalString(),
        };

        if (stream.Position != stream.Length)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Unexpected trailing {nameof(OutOfSyncMessage)} data at position {stream.Position} of {stream.Length}."
                )
            );

        return message;
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var stream = MessageStream.Create();

        stream.WriteBoolean(HasDiagnostics);
        stream.WriteOptionalString(ServerChecksum);
        stream.WriteOptionalString(ClientChecksum);
        stream.WriteOptionalString(ServerState);
        stream.WriteOptionalString(ClientState);

        return new MessageContainer(id, version, stream);
    }
}
