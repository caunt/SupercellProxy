using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Animals;

/// <summary>Starts panic movement for the animals in one habitat.</summary>
public sealed record ScareLivestockCommand(int HabitatGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ScareLivestockCommandType;

    /// <summary>Decodes the habitat id.</summary>
    public static ScareLivestockCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int id = stream.ReadVarInt();

        return new ScareLivestockCommand(id);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(HabitatGlobalId);
    }
}
