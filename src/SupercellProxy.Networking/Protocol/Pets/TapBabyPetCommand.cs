using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Pets;

/// <summary>Makes an available Farm Pass baby pet run to another point near its house.</summary>
public sealed record TapBabyPetCommand(int PetGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.TapBabyPetCommandType;

    /// <summary>Decodes the baby-pet instance before the command metadata.</summary>
    public static TapBabyPetCommand Decode(MessageStream stream, CommandEnvironment environment)
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
