using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Pets;

/// <summary>Wakes a sleeping pet and collects its feeding reward.</summary>
public sealed record WakePetCommand(int PetGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.WakePetCommandType;

    /// <summary>Decodes the pet instance before the command metadata.</summary>
    public static WakePetCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(PetGlobalId);
    }
}
