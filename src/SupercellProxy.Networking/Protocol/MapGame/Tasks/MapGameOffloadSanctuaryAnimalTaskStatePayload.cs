using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// Represents <c language="csharp">MapGameOffloadSanctuaryAnimalTaskStatePayload</c>.
/// </summary>
public sealed record MapGameOffloadSanctuaryAnimalTaskStatePayload(int Unknown0)
    : MapGameTaskStatePayload
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameOffloadSanctuaryAnimalTaskStatePayload Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MapGameOffloadSanctuaryAnimalTaskStatePayload(stream.ReadVariableInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVariableInt(Unknown0);
    }
}
