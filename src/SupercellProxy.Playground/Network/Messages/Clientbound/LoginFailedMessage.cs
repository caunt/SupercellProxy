using Nito.Disposables.Internals;
using SupercellProxy.Playground.Network.Streams;
using SupercellProxy.Playground.Resources;
using SupercellProxy.Playground.Supercell;
using System.Text.Json;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

public record LoginFailedMessage : IMessage
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public required Type ErrorCode { get; init; }
    public string? ResourceFingerprintData { get; init; }
    public string? Reason { get; init; }
    public int Unknown1 { get; init; }
    public bool Unknown2 { get; init; }
    public string? UpdateUrl { get; init; }
    public int Unknown3 { get; init; }
    public int Unknown4 { get; init; }
    public LogicLong Unknown5 { get; init; }
    public string? Unknown6 { get; init; }
    public string? Unknown7 { get; init; }
    public string? Unknown8 { get; init; }
    public string? Unknown9 { get; init; }
    public string?[]? AssetsUrls { get; init; }
    public string? Unknown10 { get; init; }

    public IEnumerable<string> AssetsUrlsFiltered => AssetsUrls?.Where(url => !string.IsNullOrWhiteSpace(url)).WhereNotNull() ?? [];
    public ResourceFingerprint ResourceFingerprint
    {
        get
        {
            var resourceFingerprintData = ResourceFingerprintData ?? throw new InvalidOperationException($"{nameof(ResourceFingerprintData)} is null.");
            var resourceFingerprint = JsonSerializer.Deserialize<ResourceFingerprint>(resourceFingerprintData, JsonSerializerOptions) ?? throw new InvalidOperationException($"Failed to deserialize {nameof(ResourceFingerprint)} from {nameof(ResourceFingerprintData)}:\n{ResourceFingerprintData}");

            return resourceFingerprint;
        }
    }

    public static LoginFailedMessage Create(MessageContainer container)
    {
        var errorCode = (Type)container.Payload.ReadInt32();
        var resourceFingerprintData = container.Payload.ReadOptionalString();
        var reason = container.Payload.ReadOptionalString();
        var unknown1 = container.Payload.ReadInt32();
        var unknown2 = container.Payload.ReadBoolean();
        var updateUrl = container.Payload.ReadOptionalString();
        var unknown3 = container.Payload.ReadVarInt();
        var unknown4 = container.Payload.ReadVarInt();
        var unknown5 = LogicLong.Empty;
        var unknown6 = string.Empty;
        var unknown7 = string.Empty;
        var unknown8 = string.Empty;
        var unknown9 = string.Empty;

        if (container.Payload.ReadBoolean())
            unknown5 = container.Payload.ReadLogicLong();

        if (container.Payload.ReadBoolean())
            unknown6 = container.Payload.ReadOptionalString();

        if (container.Payload.ReadBoolean())
            unknown7 = container.Payload.ReadOptionalString();

        if (container.Payload.ReadBoolean())
            unknown8 = container.Payload.ReadOptionalString();

        if (container.Payload.ReadBoolean())
            unknown9 = container.Payload.ReadOptionalString();

        var assetsUrls = new string?[Math.Max(0, container.Payload.ReadInt32())];

        for (var i = 0; i < assetsUrls.Length; i++)
            assetsUrls[i] = container.Payload.ReadOptionalString();

        var unknown10 = container.Payload.ReadOptionalString();

        return new LoginFailedMessage
        {
            ErrorCode = errorCode,
            ResourceFingerprintData = resourceFingerprintData,
            Reason = reason,
            Unknown1 = unknown1,
            Unknown2 = unknown2,
            UpdateUrl = updateUrl,
            Unknown3 = unknown3,
            Unknown4 = unknown4,
            Unknown5 = unknown5,
            Unknown6 = unknown6,
            Unknown7 = unknown7,
            Unknown8 = unknown8,
            Unknown9 = unknown9,
            AssetsUrls = assetsUrls,
            Unknown10 = unknown10
        };
    }

    public MessageContainer ToContainer(ushort id, ushort version = 2)
    {
        using var supercellStream = SupercellStream.Create();

        supercellStream.WriteInt32((int)ErrorCode);
        supercellStream.WriteOptionalString(ResourceFingerprintData);
        supercellStream.WriteOptionalString(Reason);
        supercellStream.WriteInt32(Unknown1);
        supercellStream.WriteBoolean(Unknown2);
        supercellStream.WriteOptionalString(UpdateUrl);
        supercellStream.WriteVarInt(Unknown3);
        supercellStream.WriteVarInt(Unknown4);

        var hasUnknown5 = Unknown5 != LogicLong.Empty;
        supercellStream.WriteBoolean(hasUnknown5);
        if (hasUnknown5)
            supercellStream.WriteLogicLong(Unknown5);

        var hasUnknown6 = !string.IsNullOrWhiteSpace(Unknown6);
        supercellStream.WriteBoolean(hasUnknown6);
        if (hasUnknown6)
            supercellStream.WriteOptionalString(Unknown6);

        var hasUnknown7 = !string.IsNullOrWhiteSpace(Unknown7);
        supercellStream.WriteBoolean(hasUnknown7);
        if (hasUnknown7)
            supercellStream.WriteOptionalString(Unknown7);

        var hasUnknown8 = !string.IsNullOrWhiteSpace(Unknown8);
        supercellStream.WriteBoolean(hasUnknown8);
        if (hasUnknown8)
            supercellStream.WriteOptionalString(Unknown8);

        var hasUnknown9 = !string.IsNullOrWhiteSpace(Unknown9);
        supercellStream.WriteBoolean(hasUnknown9);
        if (hasUnknown9)
            supercellStream.WriteOptionalString(Unknown9);

        supercellStream.WriteInt32(AssetsUrls?.Length ?? 0);
        if (AssetsUrls is not null)
        {
            foreach (var url in AssetsUrls)
                supercellStream.WriteOptionalString(url);
        }

        supercellStream.WriteOptionalString(Unknown10);

        return new MessageContainer(id, version, supercellStream);
    }

    public enum Type : int
    {
        /// <summary>
        /// Provided credentials are invalid. This occurs when the account ID or pass token is incorrect.
        /// </summary>
        InvalidCredentials = 2,

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
        Locked = 13,

        /// <summary>
        /// Wrong AppStore sent in ClientHelloMessage.
        /// </summary>
        WrongStore = 15
    };
}
