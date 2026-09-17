using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>Advertises an existing stand using a free opportunity or a purchased advertisement credit.</summary>
public sealed record AdvertiseRoadsideListingCommand(int SlotIndex, int ExecutionPhaseCounter = -1, CommandData? DebugData0 = null, CommandData? DebugData1 = null)
    : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.AdvertiseRoadsideListingCommandType;
    /// <summary>Decodes the phase and stand index.</summary>
    public static AdvertiseRoadsideListingCommand Decode(MessageStream stream, CommandEnvironment environment)
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
