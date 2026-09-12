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
    public static AvailableServerCommandMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        byte[] payload = container.Payload.ToArray();

        Command command = CommandRegistry.Decode(container.Payload, CommandEnvironment.Production, container.Payload.CommandDataResolver);

        AvailableServerCommandMessage message = new(command);
        // Container serialization finalizes any trailing packed boolean before comparison.
        byte[] roundTrip = message.ToContainer(container.Identifier, container.Version).Payload.ToArray();
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
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        MessageStream stream = MessageStream.Create();

        try
        {
            CommandRegistry.Encode(stream, Command, CommandEnvironment.Production);

            return new MessageContainer(identifier, version, stream);
        }
        finally
        {
            stream.Dispose();
        }
    }
}
