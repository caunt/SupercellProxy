using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c language="csharp">DataReferenceValue</c>.
/// </summary>
internal sealed record DataReferenceValue(int GlobalDataId, int Value)
{
    internal static DataReferenceValue Decode(MessageStream stream) =>
        new(stream.ReadVarInt(), stream.ReadVarInt());

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(GlobalDataId);
        stream.WriteVarInt(Value);
    }
}
