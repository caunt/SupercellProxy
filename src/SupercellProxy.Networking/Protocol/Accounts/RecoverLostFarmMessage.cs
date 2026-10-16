using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts;

/// <summary>Asks the server to recover a lost farm.</summary>
/// <param name="RecoveryCode">Recovery code the player enters to get the lost farm back on this device.</param>
public sealed record RecoverLostFarmMessage(string? RecoveryCode) : CodecMessage<RecoverLostFarmMessage>, ICodecProvider<RecoverLostFarmMessage>
{
    /// <inheritdoc />
    public static ICodec<RecoverLostFarmMessage> Codec { get; } = Codecs.Composite(Codecs.OptionalString, static message => message.RecoveryCode, static recoveryCode => new RecoverLostFarmMessage(recoveryCode));
}
