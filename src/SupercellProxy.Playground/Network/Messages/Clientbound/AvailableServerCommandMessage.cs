using System.Globalization;
using SupercellProxy.Playground.Commands;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

/// <summary>
/// Represents the <c language="csharp">AvailableServerCommandMessage</c> protocol message.
/// </summary>
internal sealed record AvailableServerCommandMessage(Command Command) : IMessage
{
    /// <summary>
    /// Creates a <c language="csharp">AvailableServerCommandMessage</c> from the supplied data.
    /// </summary>
    public static AvailableServerCommandMessage Create(MessageContainer container)
    {
        var payload = container.Payload.ToArray();
        var command = CommandRegistry.Decode(
            container.Payload,
            CommandEnvironment.Production,
            container.Payload.CommandDataResolver
        );

        var encoded = MessageStream.Create();
        try
        {
            CommandRegistry.Encode(encoded, command, CommandEnvironment.Production);

            if (!payload.AsSpan().SequenceEqual(encoded.ToArray()))
                throw new InvalidDataException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"Server command {command.Type} did not encode back to the received payload."
                    )
                );
        }
        finally
        {
            encoded.Dispose();
        }

        return new AvailableServerCommandMessage(command);
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        var stream = MessageStream.Create();
        try
        {
            CommandRegistry.Encode(stream, Command, CommandEnvironment.Production);
            return new MessageContainer(id, version, stream);
        }
        finally
        {
            stream.Dispose();
        }
    }
}
