using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Balloons;

/// <summary>Pops an unpopped balloon identified by its Balloon data row.</summary>
public sealed record PopBalloonCommand(int BalloonDataGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.PopBalloonCommandType;

    /// <summary>Decodes the Balloon data id.</summary>
    public static PopBalloonCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int id = stream.ReadVarInt();

        return new PopBalloonCommand(id);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(BalloonDataGlobalId);
    }
}
