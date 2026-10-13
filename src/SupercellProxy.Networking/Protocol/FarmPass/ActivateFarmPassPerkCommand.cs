using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Activates a claimed Farm Pass perk, optionally for one parameter data row.</summary>
public sealed record ActivateFarmPassPerkCommand(int PerkDataGlobalId, int ParameterDataGlobalId) : CodecCommand<ActivateFarmPassPerkCommand>, ICommandCodecProvider<ActivateFarmPassPerkCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ActivateFarmPassPerkCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.PerkDataGlobalId,
            Codecs.VarInt,
            static command => command.ParameterDataGlobalId,
            static (perk, parameter) => new ActivateFarmPassPerkCommand(perk, parameter)
        )
    );
}
