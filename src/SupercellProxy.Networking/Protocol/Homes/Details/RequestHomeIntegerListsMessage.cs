using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Homes.Details;

/// <summary>Requests the native pair of integer lists for an optional home identifier.</summary>
public sealed record RequestHomeIntegerListsMessage(LongId? HomeId) : IMessage
{
    /// <summary>Decodes the presence flag and home identifier.</summary>
    public static RequestHomeIntegerListsMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongId? home = stream.ReadBoolean() ? stream.ReadLongId() : null;

        return stream.Position == stream.Length ? new(home)
            : throw new InvalidDataException(message: "The home integer-list request has trailing data.");
    }

    /// <summary>Encodes the presence flag and home identifier.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteBoolean(HomeId.HasValue);

        if (HomeId is { } home) stream.WriteLongId(home);
    }

    /// <summary>Omits the home identifier from routine diagnostics.</summary>
    public override string ToString()
    {
        return nameof(RequestHomeIntegerListsMessage);
    }
}
