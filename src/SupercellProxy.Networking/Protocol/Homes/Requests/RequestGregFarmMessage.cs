using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Homes.Requests;

/// <summary>Requests Greg's farm using a native language-table index.</summary>
public sealed record RequestGregFarmMessage(int LanguageIndex = 0) : IMessage
{
    /// <summary>Decodes the requested language index.</summary>
    public static RequestGregFarmMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int languageIndex = stream.ReadVarInt();

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The Greg farm request has trailing data.")
            : new(languageIndex);
    }

    /// <summary>Encodes the requested language index.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteVarInt(LanguageIndex);
    }
}
