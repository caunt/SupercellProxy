using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Fishing.Molluscs;

/// <summary>Gives Angus one stored mollusc to open for meat and possible bonus pearls.</summary>
public sealed record OpenMolluscCommand(int MolluscDataId, int AngusId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.OpenMolluscCommandType;

    /// <summary>Decodes the goods id and Angus instance id following metadata.</summary>
    public static OpenMolluscCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(MolluscDataId);
        stream.WriteVarInt(AngusId);
    }
}
