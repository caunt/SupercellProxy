using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Fishing.Molluscs;

/// <summary>Collects one grown mollusc from its existing bed.</summary>
public sealed record CollectMolluscCommand(int MolluscId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CollectMolluscCommandType;

    /// <summary>Decodes the mollusc instance id preceding command metadata.</summary>
    public static CollectMolluscCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(MolluscId);
    }
}
