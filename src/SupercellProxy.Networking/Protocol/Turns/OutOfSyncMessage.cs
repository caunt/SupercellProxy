using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Turns;

/// <summary>
/// <para>OutOfSyncMessage (23626) wire representation.</para>
/// </summary>
public sealed record OutOfSyncMessage : CodecMessage<OutOfSyncMessage>, ICodecProvider<OutOfSyncMessage>
{
    /// <inheritdoc />
    public static ICodec<OutOfSyncMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.Boolean,
            static message => message.HasDiagnostics,
            Codecs.OptionalUtf8String,
            static message => message.ServerChecksum,
            Codecs.OptionalUtf8String,
            static message => message.ClientChecksum,
            Codecs.OptionalUtf8String,
            static message => message.ServerState,
            Codecs.OptionalUtf8String,
            static message => message.ClientState,
            static (hasDiagnostics, serverChecksum, clientChecksum, serverState, clientState) => new OutOfSyncMessage
            {
                HasDiagnostics = hasDiagnostics,
                ServerChecksum = serverChecksum,
                ClientChecksum = clientChecksum,
                ServerState = serverState,
                ClientState = clientState,
            }),
        $"Unexpected trailing {nameof(OutOfSyncMessage)} data.");

    /// <summary>
    /// Gets or sets the checksum the client sent for the rejected turn, as text; <see langword="null"/> unless the server includes diagnostics.
    /// </summary>
    /// <value>Optional string, the third wire field after the server checksum, holding the client's checksum for the turn the server rejected.</value>
    public string? ClientChecksum { get; init; }

    /// <summary>
    /// Gets or sets the dump of the client's game state behind that checksum; <see langword="null"/> unless the server includes diagnostics.
    /// </summary>
    /// <value>Optional string, the last wire field, with the client-side state text the server attaches for comparison against <see cref="ServerState"/>.</value>
    public string? ClientState { get; init; }

    /// <summary>
    /// Gets or sets whether the server attached its and the client's checksums and state dumps to the message.
    /// </summary>
    /// <value><see langword="true"/> if the server filled in the checksum and state strings; otherwise, <see langword="false"/>.</value>
    /// <remarks>When the flag is <see langword="false"/>, the strings are usually absent. It is the first wire field, a Boolean.</remarks>
    public bool HasDiagnostics { get; init; }

    /// <summary>
    /// Gets or sets the checksum the server computed for the same turn, as text; <see langword="null"/> unless the server includes diagnostics.
    /// </summary>
    /// <value>Optional string, the second wire field, holding the server's own checksum for the turn that did not match.</value>
    public string? ServerChecksum { get; init; }

    /// <summary>
    /// Gets or sets the dump of the server's game state behind its checksum; <see langword="null"/> unless the server includes diagnostics.
    /// </summary>
    /// <value>Optional string, the fourth wire field, with the server's textual dump of its own state for that turn.</value>
    public string? ServerState { get; init; }
}
