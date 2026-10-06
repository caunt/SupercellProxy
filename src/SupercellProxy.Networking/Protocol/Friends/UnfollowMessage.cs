using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Stops following the identified farm.</summary>
public sealed record UnfollowMessage(LongId HomeId) : IMessage
{
    /// <summary>Decodes the target farm identifier.</summary>
    public static UnfollowMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        UnfollowMessage result = new(stream.ReadLongId());

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The unfollow request has trailing data.")
            : result;
    }

    /// <summary>Encodes the target farm identifier.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteLongId(HomeId);
    }

    /// <summary>Omits the private farm identifier from diagnostics.</summary>
    public override string ToString()
    {
        return nameof(UnfollowMessage);
    }
}
