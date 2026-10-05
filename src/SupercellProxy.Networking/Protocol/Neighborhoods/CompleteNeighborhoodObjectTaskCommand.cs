using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Completes a finished Neighborhood Object task in the selected active slot.</summary>
public sealed record CompleteNeighborhoodObjectTaskCommand(int TaskSlotIndex) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CompleteNeighborhoodObjectTaskCommandType;

    /// <summary>Decodes the task slot index.</summary>
    public static CompleteNeighborhoodObjectTaskCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new CompleteNeighborhoodObjectTaskCommand(stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteVarInt(TaskSlotIndex);
    }
}
