using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

public record LoginFailedMessage : IMessage
{
    public static ushort Id => 20103;

    public enum Type : int
    {
        /// <summary>
        /// Content version is outdated. This occurs when the client's fingerprint hash is not equal
        /// to the server's fingerprint hash.
        /// </summary>
        OutdatedContent = 7,

        /// <summary>
        /// Client revision is outdated. This occurs when the client's version is not equal
        /// to the server's expected version.
        /// </summary>
        OutdatedVersion = 8,

        /// <summary>
        /// Unknown reason 1.
        /// </summary>
        Unknown1 = 9,

        /// <summary>
        /// Server is in maintenance.
        /// </summary>
        Maintenance = 10,

        /// <summary>
        /// Temporarily banned.
        /// </summary>
        TemporarilyBanned = 11,

        /// <summary>
        /// Take a rest. This occurs when the connection to the server has been maintain for too long.
        /// </summary>
        TakeRest = 12,

        /// <summary>
        /// Account has been locked. It can only be unlocked with a specific PIN.
        /// </summary>
        Locked = 13
    };

    public required Type ErrorCode { get; init; }
    public required string ResourceFingerprintData { get; init; }
    public required string RedirectDomain { get; init; }
    public required string ContentUrl { get; init; }
    public required string UpdateUrl { get; init; }
    public required string Reason { get; init; }
    public required int SecondsUntilMaintenanceEnd { get; init; }
    public required Memory<byte> UnknownBytes { get; init; }

    static IMessage IMessage.Create(MessageContainer container)
    {
        return Create(container);
    }

    public static LoginFailedMessage Create(MessageContainer container)
    {
        return new LoginFailedMessage
        {
            ErrorCode = (Type)container.Payload.ReadInt32(),
            ResourceFingerprintData = container.Payload.ReadString(),
            RedirectDomain = container.Payload.ReadString(),
            ContentUrl = container.Payload.ReadString(),
            UpdateUrl = container.Payload.ReadString(),
            Reason = container.Payload.ReadString(),
            SecondsUntilMaintenanceEnd = container.Payload.ReadInt32(),
            UnknownBytes = container.Payload.ReadToEnd()
        };
    }

    public MessageContainer ToContainer(ushort id, ushort version = 2)
    {
        using var supercellStream = SupercellStream.Create();

        supercellStream.WriteInt32((int)ErrorCode);
        supercellStream.WriteString(ResourceFingerprintData);
        supercellStream.WriteString(RedirectDomain);
        supercellStream.WriteString(ContentUrl);
        supercellStream.WriteString(UpdateUrl);
        supercellStream.WriteString(Reason);
        supercellStream.WriteInt32(SecondsUntilMaintenanceEnd);
        supercellStream.Write(UnknownBytes.Span);

        return new MessageContainer(id, version, supercellStream);
    }
}
