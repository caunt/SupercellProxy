using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Clientbound full neighborhood profiles for one directory response.</summary>
public sealed record NeighborhoodFullListMessage : IMessage
{
    /// <summary>Gets the optional ordered list of full profiles.</summary>
    public NeighborhoodProfile[]? Profiles { get; init; }

    /// <summary>Decodes the native optional profile list.</summary>
    public static NeighborhoodFullListMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;
        int count = stream.ReadVariableInt();

        if (count is < -1 or > 1024)
            throw new InvalidDataException(message: "The full neighborhood list count is invalid.");

        NeighborhoodProfile[]? profiles = count < 0 ? null : new NeighborhoodProfile[count];

        if (profiles is not null)
        {
            for (int index = 0; index < profiles.Length; index++)
                profiles[index] = NeighborhoodProfile.Decode(stream);
        }

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The full neighborhood list has trailing data.")
            : new NeighborhoodFullListMessage { Profiles = profiles };
    }

    /// <summary>Encodes the native optional profile list.</summary>
    public MessageStream ToStream()
    {
        using MessageStream stream = MessageStream.Create();

        if (Profiles?.Length is > 1024)
            throw new InvalidDataException(message: "The full neighborhood list count is invalid.");

        stream.WriteVariableInt(Profiles?.Length ?? -1);

        if (Profiles is not null)
        {
            foreach (NeighborhoodProfile profile in Profiles)
                profile.Encode(stream);
        }

        return stream;
    }

    /// <summary>Omits neighborhood names and identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(NeighborhoodFullListMessage);
    }
}
