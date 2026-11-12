using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Activates a claimed Farm Pass perk, optionally for one parameter data row.</summary>
/// <param name="PerkDataGlobalId">Global id of the perk's row in the Farm Pass perks table.</param>
/// <param name="ParameterDataGlobalId">Global id of the processing building an extra-production-slot perk is put on; 0 for every other perk type, which applies to the whole farm.</param>
public sealed record ActivateFarmPassPerkCommand(int PerkDataGlobalId, int ParameterDataGlobalId) : CodecCommand<ActivateFarmPassPerkCommand>, ICommandCodecProvider<ActivateFarmPassPerkCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ActivateFarmPassPerkCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.PerkDataGlobalId,
        Codecs.VarInt,
        static command => command.ParameterDataGlobalId,
        static (perk, parameter) => new ActivateFarmPassPerkCommand(perk, parameter)));
}
