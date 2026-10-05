using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Collects the retained drops of a completed fishing net or trap.</summary>
public sealed record CollectFishingTrapCommand(int SpotId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CollectFishingTrapCommandType;
    /// <summary>Decodes the fishing-spot id.</summary>
    public static CollectFishingTrapCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int id = stream.ReadVarInt();

        return new(id);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(SpotId);
    }
}
