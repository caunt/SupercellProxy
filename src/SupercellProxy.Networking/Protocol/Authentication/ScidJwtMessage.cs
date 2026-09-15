using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Authentication;

/// Delivers a Supercell ID JSON Web Token and its expiration time.
public sealed record ScidJwtMessage(string Token, long ExpiresAtUnixTimeSeconds) : IMessage
{
    /// Decodes a Supercell ID JSON Web Token response.
    public static ScidJwtMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        ScidJwtMessage message = new(container.Payload.ReadString(), container.Payload.ReadInt64());

        return container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "The Supercell ID JWT message has trailing data.")
            : message;
    }

    /// Encodes a Supercell ID JSON Web Token response.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteString(Token);
        stream.WriteInt64(ExpiresAtUnixTimeSeconds);

        return new MessageContainer(identifier, version, stream);
    }

    /// Omits the credential from logs.
    public override string ToString()
    {
        return $"{nameof(ScidJwtMessage)} {{ ExpiresAtUnixTimeSeconds = {ExpiresAtUnixTimeSeconds} }}";
    }
}
