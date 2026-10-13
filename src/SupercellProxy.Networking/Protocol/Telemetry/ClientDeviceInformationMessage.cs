using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>Reports the device, install and consent details after login.</summary>
/// <param name="UserAgent">The user agent.</param>
/// <param name="OsVersion">The os version.</param>
/// <param name="Locale">The locale.</param>
/// <param name="AppVersion">The app version.</param>
/// <param name="AdvertisingId">The advertising id.</param>
/// <param name="DeviceModel">The device model.</param>
/// <param name="Text6">The text6.</param>
/// <param name="Text7">The text7.</param>
/// <param name="Text8">The text8.</param>
/// <param name="InstallReferrer">The install referrer.</param>
/// <param name="Text10">The text10.</param>
/// <param name="Text11">The text11.</param>
/// <param name="Value0">The value0.</param>
/// <param name="Timestamp">The timestamp.</param>
/// <param name="Value2">The value2.</param>
/// <param name="Flag0">The flag0.</param>
/// <param name="Flag1">The flag1.</param>
/// <param name="Flag2">The flag2.</param>
/// <param name="Flag3">The flag3.</param>
/// <param name="Text12">The text12.</param>
/// <param name="Text13">The text13.</param>
/// <param name="Text14">The text14.</param>
/// <param name="Text15">The text15.</param>
/// <param name="Text16">The text16.</param>
/// <param name="Text17">The text17.</param>
/// <param name="Text18">The text18.</param>
/// <param name="Text19">The text19.</param>
/// <param name="Text20">The text20.</param>
/// <param name="Text21">The text21.</param>
/// <param name="Text22">The text22.</param>
/// <param name="Text23">The text23.</param>
/// <param name="Text24">The text24.</param>
/// <param name="Text25">The text25.</param>
/// <param name="Text26">The text26.</param>
/// <param name="Text27">The text27.</param>
/// <param name="Text28">The text28.</param>
/// <param name="Text29">The text29.</param>
/// <param name="Text30">The text30.</param>
/// <param name="InstanceId">The instance id.</param>
/// <param name="ConsentJson">The consent template states as JSON.</param>
/// <param name="ConsentControllerId">The consent controller id.</param>
/// <param name="Text34">The text34.</param>
/// <param name="Value3">The value3.</param>
public sealed record ClientDeviceInformationMessage(
    string? UserAgent,
    string? OsVersion,
    string? Locale,
    string? AppVersion,
    string? AdvertisingId,
    string? DeviceModel,
    string? Text6,
    string? Text7,
    string? Text8,
    string? InstallReferrer,
    string? Text10,
    string? Text11,
    int Value0,
    long Timestamp,
    long Value2,
    bool Flag0,
    bool Flag1,
    bool Flag2,
    bool Flag3,
    string? Text12,
    string? Text13,
    string? Text14,
    string? Text15,
    string? Text16,
    string? Text17,
    string? Text18,
    string? Text19,
    string? Text20,
    string? Text21,
    string? Text22,
    string? Text23,
    string? Text24,
    string? Text25,
    string? Text26,
    string? Text27,
    string? Text28,
    string? Text29,
    string? Text30,
    string? InstanceId,
    string? ConsentJson,
    string? ConsentControllerId,
    string? Text34,
    int Value3
) : CodecMessage<ClientDeviceInformationMessage>, ICodecProvider<ClientDeviceInformationMessage>
{
    /// <inheritdoc />
    public static ICodec<ClientDeviceInformationMessage> Codec { get; } = Codecs.Composite(
        DeviceTexts.Codec,
        static message => new DeviceTexts(
            message.UserAgent,
            message.OsVersion,
            message.Locale,
            message.AppVersion,
            message.AdvertisingId,
            message.DeviceModel,
            message.Text6,
            message.Text7,
            message.Text8,
            message.InstallReferrer,
            message.Text10,
            message.Text11
        ),
        Values.Codec,
        static message => new Values(message.Value0, message.Timestamp, message.Value2),
        Flags.Codec,
        static message => new Flags(message.Flag0, message.Flag1, message.Flag2, message.Flag3),
        ExtendedTexts.Codec,
        static message => new ExtendedTexts(
            message.Text12,
            message.Text13,
            message.Text14,
            message.Text15,
            message.Text16,
            message.Text17,
            message.Text18,
            message.Text19,
            message.Text20,
            message.Text21,
            message.Text22,
            message.Text23,
            message.Text24,
            message.Text25,
            message.Text26,
            message.Text27
        ),
        IdentityTexts.Codec,
        static message => new IdentityTexts(
            message.Text28,
            message.Text29,
            message.Text30,
            message.InstanceId,
            message.ConsentJson,
            message.ConsentControllerId,
            message.Text34
        ),
        Codecs.Int32,
        static message => message.Value3,
        static (deviceTexts, values, flags, extendedTexts, identityTexts, value3) => new ClientDeviceInformationMessage(
            deviceTexts.UserAgent,
            deviceTexts.OsVersion,
            deviceTexts.Locale,
            deviceTexts.AppVersion,
            deviceTexts.AdvertisingId,
            deviceTexts.DeviceModel,
            deviceTexts.Text6,
            deviceTexts.Text7,
            deviceTexts.Text8,
            deviceTexts.InstallReferrer,
            deviceTexts.Text10,
            deviceTexts.Text11,
            values.Value0,
            values.Timestamp,
            values.Value2,
            flags.Flag0,
            flags.Flag1,
            flags.Flag2,
            flags.Flag3,
            extendedTexts.Text12,
            extendedTexts.Text13,
            extendedTexts.Text14,
            extendedTexts.Text15,
            extendedTexts.Text16,
            extendedTexts.Text17,
            extendedTexts.Text18,
            extendedTexts.Text19,
            extendedTexts.Text20,
            extendedTexts.Text21,
            extendedTexts.Text22,
            extendedTexts.Text23,
            extendedTexts.Text24,
            extendedTexts.Text25,
            extendedTexts.Text26,
            extendedTexts.Text27,
            identityTexts.Text28,
            identityTexts.Text29,
            identityTexts.Text30,
            identityTexts.InstanceId,
            identityTexts.ConsentJson,
            identityTexts.ConsentControllerId,
            identityTexts.Text34,
            value3
        )
    );

    // The device and app texts native reads first.
    private readonly record struct DeviceTexts(
        string? UserAgent,
        string? OsVersion,
        string? Locale,
        string? AppVersion,
        string? AdvertisingId,
        string? DeviceModel,
        string? Text6,
        string? Text7,
        string? Text8,
        string? InstallReferrer,
        string? Text10,
        string? Text11
    )
    {
        public static ICodec<DeviceTexts> Codec { get; } = Codecs.Composite(
            Codecs.OptionalString,
            static segment => segment.UserAgent,
            Codecs.OptionalString,
            static segment => segment.OsVersion,
            Codecs.OptionalString,
            static segment => segment.Locale,
            Codecs.OptionalString,
            static segment => segment.AppVersion,
            Codecs.OptionalString,
            static segment => segment.AdvertisingId,
            Codecs.OptionalString,
            static segment => segment.DeviceModel,
            Codecs.OptionalString,
            static segment => segment.Text6,
            Codecs.OptionalString,
            static segment => segment.Text7,
            Codecs.OptionalString,
            static segment => segment.Text8,
            Codecs.OptionalString,
            static segment => segment.InstallReferrer,
            Codecs.OptionalString,
            static segment => segment.Text10,
            Codecs.OptionalString,
            static segment => segment.Text11,
            static (userAgent, osVersion, locale, appVersion, advertisingId, deviceModel, text6, text7, text8, installReferrer, text10, text11) => new DeviceTexts(userAgent, osVersion, locale, appVersion, advertisingId, deviceModel, text6, text7, text8, installReferrer, text10, text11)
        );
    }

    // The first sixteen texts native reads after the flags.
    private readonly record struct ExtendedTexts(
        string? Text12,
        string? Text13,
        string? Text14,
        string? Text15,
        string? Text16,
        string? Text17,
        string? Text18,
        string? Text19,
        string? Text20,
        string? Text21,
        string? Text22,
        string? Text23,
        string? Text24,
        string? Text25,
        string? Text26,
        string? Text27
    )
    {
        public static ICodec<ExtendedTexts> Codec { get; } = Codecs.Composite(
            Codecs.OptionalString,
            static segment => segment.Text12,
            Codecs.OptionalString,
            static segment => segment.Text13,
            Codecs.OptionalString,
            static segment => segment.Text14,
            Codecs.OptionalString,
            static segment => segment.Text15,
            Codecs.OptionalString,
            static segment => segment.Text16,
            Codecs.OptionalString,
            static segment => segment.Text17,
            Codecs.OptionalString,
            static segment => segment.Text18,
            Codecs.OptionalString,
            static segment => segment.Text19,
            Codecs.OptionalString,
            static segment => segment.Text20,
            Codecs.OptionalString,
            static segment => segment.Text21,
            Codecs.OptionalString,
            static segment => segment.Text22,
            Codecs.OptionalString,
            static segment => segment.Text23,
            Codecs.OptionalString,
            static segment => segment.Text24,
            Codecs.OptionalString,
            static segment => segment.Text25,
            Codecs.OptionalString,
            static segment => segment.Text26,
            Codecs.OptionalString,
            static segment => segment.Text27,
            static (text12, text13, text14, text15, text16, text17, text18, text19, text20, text21, text22, text23, text24, text25, text26, text27) => new ExtendedTexts(text12, text13, text14, text15, text16, text17, text18, text19, text20, text21, text22, text23, text24, text25, text26, text27)
        );
    }

    // The four flags native reads together.
    private readonly record struct Flags(bool Flag0, bool Flag1, bool Flag2, bool Flag3)
    {
        public static ICodec<Flags> Codec { get; } = Codecs.Composite(
            Codecs.Boolean,
            static segment => segment.Flag0,
            Codecs.Boolean,
            static segment => segment.Flag1,
            Codecs.Boolean,
            static segment => segment.Flag2,
            Codecs.Boolean,
            static segment => segment.Flag3,
            static (flag0, flag1, flag2, flag3) => new Flags(flag0, flag1, flag2, flag3)
        );
    }

    // The texts native reads before the trailing value, including the instance and consent ids.
    private readonly record struct IdentityTexts(
        string? Text28,
        string? Text29,
        string? Text30,
        string? InstanceId,
        string? ConsentJson,
        string? ConsentControllerId,
        string? Text34
    )
    {
        public static ICodec<IdentityTexts> Codec { get; } = Codecs.Composite(
            Codecs.OptionalString,
            static segment => segment.Text28,
            Codecs.OptionalString,
            static segment => segment.Text29,
            Codecs.OptionalString,
            static segment => segment.Text30,
            Codecs.OptionalString,
            static segment => segment.InstanceId,
            Codecs.OptionalString,
            static segment => segment.ConsentJson,
            Codecs.OptionalString,
            static segment => segment.ConsentControllerId,
            Codecs.OptionalString,
            static segment => segment.Text34,
            static (text28, text29, text30, instanceId, consentJson, consentControllerId, text34) => new IdentityTexts(text28, text29, text30, instanceId, consentJson, consentControllerId, text34)
        );
    }

    // The values native reads after the device texts.
    private readonly record struct Values(int Value0, long Timestamp, long Value2)
    {
        public static ICodec<Values> Codec { get; } = Codecs.Composite(
            Codecs.VarInt,
            static segment => segment.Value0,
            Codecs.Int64,
            static segment => segment.Timestamp,
            Codecs.Int64,
            static segment => segment.Value2,
            static (value0, timestamp, value2) => new Values(value0, timestamp, value2)
        );
    }
}
