using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Sanctuary;

/// <summary>Calls compatible sanctuary animals home and collects rewards from animals woken by the call.</summary>
public sealed record CallSanctuaryAnimalsToHabitatCommand(int HabitatGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CallSanctuaryAnimalsToHabitatCommandType;

    /// <summary>Decodes the habitat instance before the command metadata.</summary>
    public static CallSanctuaryAnimalsToHabitatCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(HabitatGlobalId);
    }
}
