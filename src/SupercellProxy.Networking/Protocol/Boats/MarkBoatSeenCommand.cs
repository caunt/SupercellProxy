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
    public MarkBoatSeenCommand(bool nextBoat)
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

        return new MarkBoatSeenCommand(stream.ReadBoolean());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteBoolean(NextBoat);
    }
}
