using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Characters;

/// <summary>Collects Angus's accumulated pearls from the pearl bucket.</summary>
public sealed record CollectAngusPearlsCommand(int BucketGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CollectAngusPearlsCommandType;

    /// <summary>Decodes the pearl bucket instance.</summary>
    public static CollectAngusPearlsCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new CollectAngusPearlsCommand(stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(BucketGlobalId);
    }
}
