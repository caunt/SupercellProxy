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
    /// The checksum the client sent for the rejected turn, as text; null unless the server includes diagnostics.
    /// </summary>
    public string? ClientChecksum { get; init; }

    /// <summary>
    /// Dump of the client's game state behind that checksum; null unless the server includes diagnostics.
    /// </summary>
    public string? ClientState { get; init; }
    /// <summary>
    /// Whether the server attached its and the client's checksums and state dumps to the message.
    /// </summary>
    public bool HasDiagnostics { get; init; }

    /// <summary>
    /// The checksum the server computed for the same turn, as text; null unless the server includes diagnostics.
    /// </summary>
    public string? ServerChecksum { get; init; }

    /// <summary>
    /// Dump of the server's game state behind its checksum; null unless the server includes diagnostics.
    /// </summary>
    public string? ServerState { get; init; }
}
