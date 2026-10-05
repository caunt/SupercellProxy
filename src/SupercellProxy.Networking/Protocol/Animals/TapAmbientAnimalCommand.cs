using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Animals;

/// <summary>Taps a registered ambient animal to resume its movement.</summary>
public sealed record TapAmbientAnimalCommand(int AmbientAnimalGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.TapAmbientAnimalCommandType;

    /// <summary>Decodes the runtime animal id.</summary>
    public static TapAmbientAnimalCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int id = stream.ReadVarInt();

        return new TapAmbientAnimalCommand(id);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(AmbientAnimalGlobalId);
    }
}
