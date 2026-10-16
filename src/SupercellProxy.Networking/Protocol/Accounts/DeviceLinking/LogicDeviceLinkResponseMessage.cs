using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.DeviceLinking;

/// <summary>Answers a device link request; error codes 1, 2, 5, 6 and 7 show a dialog and zero opens the link result.</summary>
/// <param name="ErrorCode">0 when the link went through; 1, 2, 5, 6 and 7 make the client show an error dialog.</param>
/// <param name="Text">First text the response carries; its role in the link result is not identified.</param>
/// <param name="SecondText">Second text the response carries; not identified.</param>
/// <param name="Value">Number sent between the texts; not identified.</param>
/// <param name="ThirdText">Third text the response carries; not identified.</param>
/// <param name="AccountId">Account the link concerns.</param>
public sealed record LogicDeviceLinkResponseMessage(int ErrorCode, string? Text, string? SecondText, int Value, string? ThirdText, LongId AccountId) : CodecMessage<LogicDeviceLinkResponseMessage>, ICodecProvider<LogicDeviceLinkResponseMessage>
{
    /// <inheritdoc />
    public static ICodec<LogicDeviceLinkResponseMessage> Codec { get; } = Codecs.Composite(
        Codecs.Int32,
        static message => message.ErrorCode,
        Codecs.OptionalString,
        static message => message.Text,
        Codecs.OptionalString,
        static message => message.SecondText,
        Codecs.Int32,
        static message => message.Value,
        Codecs.OptionalString,
        static message => message.ThirdText,
        Codecs.LongId,
        static message => message.AccountId,
        static (errorCode, text, secondText, @value, thirdText, accountId) => new LogicDeviceLinkResponseMessage(errorCode, text, secondText, value, thirdText, accountId)
    );
}
