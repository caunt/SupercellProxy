using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Turns;

/// <summary>
/// <para>OutOfSyncMessage (23626) wire representation.</para>
/// </summary>
public sealed record OutOfSyncMessage : CodecMessage<OutOfSyncMessage>, ICodecProvider<OutOfSyncMessage>
{
    /// <inheritdoc />
    public static ICodec<OutOfSyncMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.Boolean,
            static message => message.HasDiagnostics,
            Codecs.OptionalString,
            static message => message.ServerChecksum,
            Codecs.OptionalString,
            static message => message.ClientChecksum,
            Codecs.OptionalString,
            static message => message.ServerState,
            Codecs.OptionalString,
            static message => message.ClientState,
            static (hasDiagnostics, serverChecksum, clientChecksum, serverState, clientState) => new OutOfSyncMessage
            {
                HasDiagnostics = hasDiagnostics,
                ServerChecksum = serverChecksum,
                ClientChecksum = clientChecksum,
                ServerState = serverState,
                ClientState = clientState,
            }
        ),
        $"Unexpected trailing {nameof(OutOfSyncMessage)} data."
    );

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
}
