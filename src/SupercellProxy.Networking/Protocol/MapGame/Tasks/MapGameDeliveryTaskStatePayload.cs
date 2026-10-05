using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// Represents <c language="csharp">MapGameDeliveryTaskStatePayload</c>.
/// </summary>
public sealed record MapGameDeliveryTaskStatePayload(int Unknown0, int Unknown1, bool UnknownBoolean0) : MapGameTaskStatePayload
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameDeliveryTaskStatePayload Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MapGameDeliveryTaskStatePayload(stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadBoolean());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteBoolean(UnknownBoolean0);
    }
}
