using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Requests the selected waiting passengers from the currently visited Town.</summary>
public sealed record RequestTownPassengerPickupMessage(int[]? HelpIds) : IMessage
{
    /// <summary>Decodes the nullable list of passenger help identifiers.</summary>
    public static RequestTownPassengerPickupMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = stream.ReadVarInt();

        if (count < 0 || count > 10_000) return new(HelpIds: null);

        int[] ids = new int[count];

        for (int index = 0; index < ids.Length; index++) ids[index] = stream.ReadVarInt();

        return new(ids);
    }

    /// <summary>Encodes the selected passenger identifiers, preserving an absent list.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (HelpIds is { } ids)
            stream.WriteArray(ids, static (output, id) => output.WriteVarInt(id));
        else
            stream.WriteVarInt(valueToWrite: -1);
    }
}
