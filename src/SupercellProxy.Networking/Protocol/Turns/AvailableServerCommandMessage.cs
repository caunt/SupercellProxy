using System.Globalization;

using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Turns;

/// <summary>
/// Represents the <c language="csharp">AvailableServerCommandMessage</c> protocol message.
/// </summary>
public sealed record AvailableServerCommandMessage(Command Command) : IMessage
{
    /// <summary>
    /// Creates a <c language="csharp">AvailableServerCommandMessage</c> from the supplied data.
    /// </summary>
    public static AvailableServerCommandMessage Decode(MessageStream stream)
    {
        return Decode(stream, dataResolver: null);
    }

    /// <summary>
    /// Creates a <c language="csharp">AvailableServerCommandMessage</c> from the supplied data.
    /// </summary>
    public static AvailableServerCommandMessage Decode(MessageStream stream, ICommandDataResolver? dataResolver)
    {
        ArgumentNullException.ThrowIfNull(stream);
        byte[] payload = stream.ToArray();

        Command command = CommandRegistry.Decode(stream, CommandEnvironment.Production, dataResolver ?? stream.CommandDataResolver);

        AvailableServerCommandMessage message = new(command);
        // Container serialization finalizes any trailing packed boolean before comparison.
        byte[] roundTrip = MessageContainer.Create(message).Payload.ToArray();
        int difference = 0;

        while (difference < payload.Length && difference < roundTrip.Length && payload[difference] == roundTrip[difference])
            difference++;

        return !payload.AsSpan().SequenceEqual(roundTrip)
            ? throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Server command {command.Type} did not encode back to the received payload; first difference {difference} ({payload.ElementAtOrDefault(difference)} versus {roundTrip.ElementAtOrDefault(difference)}), received length {payload.Length}, encoded length {roundTrip.Length}."
                )
            )
            : message;
    }

    /// <summary>
    /// Executes the <c language="csharp">Encode</c> operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        CommandRegistry.Encode(stream, Command, CommandEnvironment.Production);
    }
}
