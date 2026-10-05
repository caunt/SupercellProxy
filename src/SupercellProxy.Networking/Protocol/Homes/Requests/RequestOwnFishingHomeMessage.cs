using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Homes.Requests;

/// <summary>Requests the authenticated player's native fishing-area snapshot.</summary>
public sealed record RequestOwnFishingHomeMessage : IMessage
{
    /// <summary>Decodes the captured empty fishing request.</summary>
    public static RequestOwnFishingHomeMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return container.Payload.Length != 0
            ? throw new InvalidDataException(message: "The own-fishing-home request must have an empty payload.")
            : new();
    }

    /// <summary>Encodes the empty request using its registry-owned identifier.</summary>
    public MessageStream ToStream()
    {
        using MessageStream stream = MessageStream.Create();

        return stream;
    }
}
