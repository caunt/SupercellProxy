using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Pets;

/// <summary>Calls compatible pets to a habitat and collects rewards from pets woken by the call.</summary>
public sealed record CallPetsToHabitatCommand(int HabitatGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CallPetsToHabitatCommandType;

    /// <summary>Decodes the habitat instance before the command metadata.</summary>
    public static CallPetsToHabitatCommand Decode(MessageStream stream, CommandEnvironment environment)
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
