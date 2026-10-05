using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Activates a claimed Farm Pass perk, optionally for one parameter data row.</summary>
public sealed record ActivateFarmPassPerkCommand(int PerkDataGlobalId, int ParameterDataGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ActivateFarmPassPerkCommandType;

    /// <summary>Decodes a Farm Pass perk activation command.</summary>
    public static ActivateFarmPassPerkCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int perk = stream.ReadVarInt();
        int parameter = stream.ReadVarInt();

        return new ActivateFarmPassPerkCommand(perk, parameter);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(PerkDataGlobalId);
        stream.WriteVarInt(ParameterDataGlobalId);
    }
}
