using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Creatures;

/// <summary>Starts catching an existing seasonal creature.</summary>
public sealed record CatchCreatureCommand(int CreatureGlobalId) : Command
{
    /// <summary>Gets the native command id.</summary>
    public override int Type => CommandRegistry.CatchCreatureCommandType;

    /// <summary>Decodes the creature instance id.</summary>
    public static CatchCreatureCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt());
    }

    /// <summary>Encodes the creature instance id.</summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(CreatureGlobalId);
    }
}
