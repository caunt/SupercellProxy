using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Help;

/// <summary>Sends neighborhood help.</summary>
/// <param name="Completed">The completed.</param>
/// <param name="HelpKind">The help kind.</param>
/// <param name="HelpId">The help id.</param>
/// <param name="RewardIndex">The reward index.</param>
public sealed record SendNeighborhoodHelpMessage(bool Completed, int HelpKind, int HelpId, int RewardIndex) : CodecMessage<SendNeighborhoodHelpMessage>, ICodecProvider<SendNeighborhoodHelpMessage>
{
    /// <inheritdoc />
    public static ICodec<SendNeighborhoodHelpMessage> Codec { get; } = Codecs.Composite(
        Codecs.Boolean,
        static message => message.Completed,
        Codecs.VarInt,
        static message => message.HelpKind,
        Codecs.VarInt,
        static message => message.HelpId,
        Codecs.VarInt,
        static message => message.RewardIndex,
        static (completed, helpKind, helpId, rewardIndex) => new SendNeighborhoodHelpMessage(completed, helpKind, helpId, rewardIndex)
    );
}
