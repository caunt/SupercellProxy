using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c language="csharp">PickedPassenger</c>.
/// </summary>
/// <param name="Unknown0">The <c language="csharp">Unknown0</c> value.</param>
/// <param name="Unknown1">The <c language="csharp">Unknown1</c> value.</param>
/// <param name="Unknown2">The <c language="csharp">Unknown2</c> value.</param>
/// <param name="UnknownId0">The <c language="csharp">UnknownId0</c> value.</param>
/// <param name="UnknownId1">The <c language="csharp">UnknownId1</c> value.</param>
/// <param name="UnknownString0">The <c language="csharp">UnknownString0</c> value.</param>
internal sealed record PickedPassenger(
    int Unknown0,
    int Unknown1,
    int Unknown2,
    LongId UnknownId0,
    LongId UnknownId1,
    string? UnknownString0
)
{
    internal static PickedPassenger Decode(MessageStream stream) =>
        new(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadLongId(),
            stream.ReadLongId(),
            stream.ReadOptionalString()
        );

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        stream.WriteLongId(UnknownId0);
        stream.WriteLongId(UnknownId1);
        stream.WriteOptionalString(UnknownString0);
    }
}
