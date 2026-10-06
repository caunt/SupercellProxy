using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Asks whether the visitor has already liked the identified farm.</summary>
public sealed record RequestFarmLikeStatusMessage(LongId HomeOwnerId, LongId VisitorId) : IMessage
{
    /// <summary>Decodes the target farm and visitor identifiers.</summary>
    public static RequestFarmLikeStatusMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        RequestFarmLikeStatusMessage result = new(stream.ReadLongId(), stream.ReadLongId());

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The farm-like status request has trailing data.")
            : result;
    }

    /// <summary>Encodes the target farm before the visitor.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteLongId(HomeOwnerId);
        stream.WriteLongId(VisitorId);
    }

    /// <summary>Omits the private identifiers from diagnostics.</summary>
    public override string ToString()
    {
        return nameof(RequestFarmLikeStatusMessage);
    }
}
