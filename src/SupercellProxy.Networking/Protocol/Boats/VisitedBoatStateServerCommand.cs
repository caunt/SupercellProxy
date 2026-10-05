using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Defines the Visited Boat State Server Command contract.
/// </summary>
/// <summary>
/// Defines the New State contract.
/// </summary>
/// <summary>
/// Defines the Expected State contract.
/// </summary>
/// <summary>
/// Defines the Order Index contract.
/// </summary>
/// <summary>
/// Defines the Owner High contract.
/// </summary>
/// <summary>
/// Defines the Owner Low contract.
/// </summary>
/// <summary>
/// Defines the State Duration contract.
/// </summary>
/// <summary>
/// Defines the State Ticks contract.
/// </summary>
public sealed record VisitedBoatStateServerCommand(int NewState, int ExpectedState, int OrderIndex, int OwnerHigh, int OwnerLow, int StateDuration, int StateTicks) : ServerCommand
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.VisitedBoatStateServerCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static VisitedBoatStateServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);


        return new VisitedBoatStateServerCommand(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt()
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(NewState);
        stream.WriteVarInt(ExpectedState);
        stream.WriteVarInt(OrderIndex);
        stream.WriteVarInt(OwnerHigh);
        stream.WriteVarInt(OwnerLow);
        stream.WriteVarInt(StateDuration);
        stream.WriteVarInt(StateTicks);
    }
}
