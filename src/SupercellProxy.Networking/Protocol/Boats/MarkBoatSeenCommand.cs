using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Defines the Mark Boat Seen Command contract.
/// </summary>
public sealed record MarkBoatSeenCommand : Command
{
    /// <summary>
    /// Provides the Command Type value or operation.
    /// </summary>
    public const int CommandType = 600;

    /// <summary>
    /// Provides the Mark Boat Seen Command value or operation.
    /// </summary>
    public MarkBoatSeenCommand(bool nextBoat, int executionPhaseCounter = 0, CommandData? debugData0 = null, CommandData? debugData1 = null)
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        NextBoat = nextBoat;
    }

    /// <summary>
    /// Gets the Next Boat value.
    /// </summary>
    public bool NextBoat { get; }

    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MarkBoatSeenCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new MarkBoatSeenCommand(stream.ReadBoolean(), fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteBoolean(NextBoat);
    }
}
