using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Removes a departing duck or lobster from its processing facility.</summary>
public sealed record RemoveFishingAnimalCommand(int AnimalId, bool Duck) : Command
{
    /// <inheritdoc />
    public override int Type => Duck ? CommandRegistry.RemoveDuckCommandType : CommandRegistry.RemoveLobsterCommandType;

    /// <summary>Decodes the animal instance before the shared command fields.</summary>
    public static RemoveFishingAnimalCommand Decode(MessageStream stream, CommandEnvironment environment, bool duck)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new RemoveFishingAnimalCommand(stream.ReadVarInt(), duck);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(AnimalId);
    }
}
