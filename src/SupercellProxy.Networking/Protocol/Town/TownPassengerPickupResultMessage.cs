using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Reports a personal-train pickup request's result for a visited Town.</summary>
public sealed record TownPassengerPickupResultMessage(LongId? PickingHomeId, LongId? VisitedHomeId, int Unknown0, int Status) : IMessage
{
    /// <summary>Decodes the two optional home identifiers and result fields.</summary>
    public static TownPassengerPickupResultMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongId? pickingHome = stream.ReadBoolean() ? stream.ReadLongId() : null;
        LongId? visitedHome = stream.ReadBoolean() ? stream.ReadLongId() : null;

        return new(pickingHome, visitedHome, stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <summary>Encodes the native optional identifiers and result fields.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteBoolean(PickingHomeId.HasValue);

        if (PickingHomeId is { } picking) stream.WriteLongId(picking);

        stream.WriteBoolean(VisitedHomeId.HasValue);

        if (VisitedHomeId is { } visited) stream.WriteLongId(visited);

        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Status);
    }
}
