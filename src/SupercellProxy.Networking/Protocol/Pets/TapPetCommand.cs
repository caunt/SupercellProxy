using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Pets;

/// <summary>Makes an awake pet without an active care movement run around its habitat.</summary>
public sealed record TapPetCommand(int PetGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.TapPetCommandType;

    /// <summary>Decodes the pet instance before the command metadata.</summary>
    public static TapPetCommand Decode(MessageStream stream, CommandEnvironment environment)
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
