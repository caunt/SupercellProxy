using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// <para>Native three-value structure embedded in a map-game pawn. Its semantic field names are not present in the stripped client.</para>
/// </summary>
public sealed record MapGamePawnNestedData(int Unknown0, int Unknown1, int Unknown2)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGamePawnNestedData Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MapGamePawnNestedData(stream.ReadVariableInt(), stream.ReadVariableInt(), stream.ReadVariableInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(Unknown0);
        stream.WriteVariableInt(Unknown1);
        stream.WriteVariableInt(Unknown2);
    }
}
