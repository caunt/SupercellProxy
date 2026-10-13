using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Seen;

/// <summary>Records that the Farm Pass intro was shown.</summary>
public sealed record MarkFarmPassIntroSeenCommand : CodecCommand<MarkFarmPassIntroSeenCommand>, ICommandCodecProvider<MarkFarmPassIntroSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkFarmPassIntroSeenCommand> Codec { get; } = Codecs.Command(type: 2612, Codecs.Unit(static () => new MarkFarmPassIntroSeenCommand()));
}
