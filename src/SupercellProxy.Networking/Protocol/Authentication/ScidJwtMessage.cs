using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Authentication;

/// Delivers a Supercell ID JSON Web Token and its expiration time.
public sealed record ScidJwtMessage(string Token, long ExpiresAtUnixTimeSeconds) : IMessage
{
    /// Decodes a Supercell ID JSON Web Token response.
    public static ScidJwtMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        ScidJwtMessage message = new(stream.ReadString(), stream.ReadInt64());

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The Supercell ID JWT message has trailing data.")
            : message;
    }

    /// Encodes a Supercell ID JSON Web Token response.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteString(Token);
        stream.WriteInt64(ExpiresAtUnixTimeSeconds);
    }

    /// Omits the credential from logs.
    public override string ToString()
    {
        return $"{nameof(ScidJwtMessage)} {{ ExpiresAtUnixTimeSeconds = {ExpiresAtUnixTimeSeconds} }}";
    }
}
