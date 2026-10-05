using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Homes.Requests;

/// <summary>Requests Greg's farm using a native language-table index.</summary>
public sealed record RequestGregFarmMessage(int LanguageIndex = 0) : IMessage
{
    /// <summary>Decodes the requested language index.</summary>
    public static RequestGregFarmMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;
        int languageIndex = stream.ReadVariableInt();

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The Greg farm request has trailing data.")
            : new(languageIndex);
    }

    /// <summary>Encodes the requested language index.</summary>
    public MessageStream ToStream()
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(LanguageIndex);

        return stream;
    }
}
