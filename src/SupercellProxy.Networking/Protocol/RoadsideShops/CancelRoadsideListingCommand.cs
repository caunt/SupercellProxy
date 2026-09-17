using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>Cancels an unsold listing; native configuration determines item return and diamond cost.</summary>
public sealed record CancelRoadsideListingCommand(int SlotIndex, int ExecutionPhaseCounter = -1, CommandData? DebugData0 = null, CommandData? DebugData1 = null)
    : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CancelRoadsideListingCommandType;
    /// <summary>Decodes the phase and stand index.</summary>
    public static CancelRoadsideListingCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        (int phase, CommandData? debug0, CommandData? debug1) = DecodeCommand(stream, environment);

        return new(stream.ReadVariableInt(), phase, debug0, debug1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteVariableInt(SlotIndex);
    }
}
