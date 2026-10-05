using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// Carries encoded <c language="csharp">AvatarStateValues</c> wire values.
/// </summary>
public sealed record AvatarStateValues(int Unknown0, int Unknown1, int Unknown2, long Unknown3)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AvatarStateValues Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadInt64());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        stream.WriteInt64(Unknown3);
    }
}
