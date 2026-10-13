using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Reengagement;

/// <summary>Advances the returning-player welcome flow to its next enabled step, granting the stored coin and diamond rewards on the reward step.</summary>
public sealed record AdvanceReengagementFlowCommand : CodecCommand<AdvanceReengagementFlowCommand>, ICommandCodecProvider<AdvanceReengagementFlowCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AdvanceReengagementFlowCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new AdvanceReengagementFlowCommand()));
}
