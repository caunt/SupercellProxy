using System.Text.Json;
using Nito.Disposables.Internals;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Protocol;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

/// <summary>
/// Represents the <c language="csharp">LoginFailedMessage</c> protocol message.
/// </summary>
internal sealed record LoginFailedMessage : IMessage
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    /// Gets or sets the <c language="csharp">ErrorCode</c> value.
    /// </summary>
    public required LoginFailureType ErrorCode { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GameAssetFingerprintData</c> value.
    /// </summary>
    public string? GameAssetFingerprintData { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Reason</c> value.
    /// </summary>
    public string? Reason { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown2</c> value.
    /// </summary>
    public bool Unknown2 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UpdateUrl</c> value.
    /// </summary>
    public string? UpdateUrl { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown3</c> value.
    /// </summary>
    public int Unknown3 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown4</c> value.
    /// </summary>
    public int Unknown4 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown5</c> value.
    /// </summary>
    public LongId Unknown5 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown6</c> value.
    /// </summary>
    public string? Unknown6 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown7</c> value.
    /// </summary>
    public string? Unknown7 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown8</c> value.
    /// </summary>
    public string? Unknown8 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown9</c> value.
    /// </summary>
    public string? Unknown9 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AssetsUrls</c> value.
    /// </summary>
    public string?[]? AssetsUrls { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">RedirectHost</c> value.
    /// </summary>
    public string? RedirectHost { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">AssetsUrlsFiltered</c> value.
    /// </summary>
    public IEnumerable<string> AssetsUrlsFiltered =>
        AssetsUrls?.Where(static url => !string.IsNullOrWhiteSpace(url)).WhereNotNull() ?? [];

    /// <summary>
    /// Gets the <c language="csharp">GameAssetFingerprint</c> value.
    /// </summary>
    public GameAssetFingerprint GameAssetFingerprint
    {
        get
        {
            var resourceFingerprintData =
                GameAssetFingerprintData
                ?? throw new InvalidOperationException(
                    $"{nameof(GameAssetFingerprintData)} is null."
                );
            var resourceFingerprint =
                JsonSerializer.Deserialize<GameAssetFingerprint>(
                    resourceFingerprintData,
                    JsonSerializerOptions
                )
                ?? throw new InvalidOperationException(
                    $"Failed to deserialize {nameof(GameAssetFingerprint)} from {nameof(GameAssetFingerprintData)}:\n{GameAssetFingerprintData}"
                );

            return resourceFingerprint;
        }
    }

    /// <summary>
    /// Creates a <c language="csharp">LoginFailedMessage</c> from the supplied data.
    /// </summary>
    public static LoginFailedMessage Create(MessageContainer container)
    {
        var errorCode = System.Runtime.CompilerServices.Unsafe.BitCast<int, LoginFailureType>(
            container.Payload.ReadInt32()
        );
        var resourceFingerprintData = container.Payload.ReadOptionalString();
        var reason = container.Payload.ReadOptionalString();
        var unknown1 = container.Payload.ReadInt32();
        var unknown2 = container.Payload.ReadBoolean();
        var updateUrl = container.Payload.ReadOptionalString();
        var unknown3 = container.Payload.ReadVarInt();
        var unknown4 = container.Payload.ReadVarInt();
        var unknown5 = LongId.Empty;
        var unknown6 = string.Empty;
        var unknown7 = string.Empty;
        var unknown8 = string.Empty;
        var unknown9 = string.Empty;

        if (container.Payload.ReadBoolean())
            unknown5 = container.Payload.ReadLongId();

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

        var redirectHost = container.Payload.ReadOptionalString();

        return new LoginFailedMessage
        {
            ErrorCode = errorCode,
            GameAssetFingerprintData = resourceFingerprintData,
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
            RedirectHost = redirectHost,
        };
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var supercellStream = MessageStream.Create();

        supercellStream.WriteInt32(
            System.Runtime.CompilerServices.Unsafe.BitCast<LoginFailureType, int>(ErrorCode)
        );
        supercellStream.WriteOptionalString(GameAssetFingerprintData);
        supercellStream.WriteOptionalString(Reason);
        supercellStream.WriteInt32(Unknown1);
        supercellStream.WriteBoolean(Unknown2);
        supercellStream.WriteOptionalString(UpdateUrl);
        supercellStream.WriteVarInt(Unknown3);
        supercellStream.WriteVarInt(Unknown4);

        var hasUnknown5 = Unknown5 != LongId.Empty;
        supercellStream.WriteBoolean(hasUnknown5);
        if (hasUnknown5)
            supercellStream.WriteLongId(Unknown5);

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

        supercellStream.WriteOptionalString(RedirectHost);

        return new MessageContainer(id, version, supercellStream);
    }
}
