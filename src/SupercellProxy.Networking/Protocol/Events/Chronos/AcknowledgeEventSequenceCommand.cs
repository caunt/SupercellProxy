using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>Acknowledges the presentation sequence following an event reward.</summary>
public sealed record AcknowledgeEventSequenceCommand(int StepIndex, int EventId) : Command
{
    /// <inheritdoc/>
    public override int Type => CommandRegistry.AcknowledgeEventSequenceCommandType;

    /// <inheritdoc/>
    public static AcknowledgeEventSequenceCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <inheritdoc/>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(StepIndex);
        stream.WriteVarInt(EventId);
    }
}
