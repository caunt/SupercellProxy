using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Pets;

/// <summary>Applies a server-approved name to a pet or sanctuary animal.</summary>
public sealed record RenameAnimalServerCommand(int AnimalGlobalId, string Name, bool Approved) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.RenameAnimalServerCommandType;

    /// <summary>Decodes the target, name and approval before server-command metadata.</summary>
    public static RenameAnimalServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadInt32(), stream.ReadString(), stream.ReadBoolean());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteInt32(AnimalGlobalId);
        stream.WriteString(Name);
        stream.WriteBoolean(Approved);
    }
}
