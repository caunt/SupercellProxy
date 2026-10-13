using System.Globalization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Turns;

/// <summary>
/// Represents the <c language="csharp">AvailableServerCommandMessage</c> protocol message.
/// </summary>
public sealed record AvailableServerCommandMessage(Command Command) : CodecMessage<AvailableServerCommandMessage>, ICodecProvider<AvailableServerCommandMessage>
{
    /// <inheritdoc />
    /// <remarks>The command must encode back to the received payload byte for byte.</remarks>
    public static ICodec<AvailableServerCommandMessage> Codec { get; } = Codecs.Create(
        static stream =>
        {
            byte[] payload = stream.ToArray();
            Command command = CommandRegistry.Codec.Read(stream);
            AvailableServerCommandMessage message = new(command);

            // Container serialization finalizes any trailing packed boolean before comparison.
            byte[] roundTrip = MessageContainer.Create(message, gameVersion: stream.GameVersion).Payload.ToArray();
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
        },
        static (stream, message) => CommandRegistry.Codec.Write(stream, message.Command)
    );
}
