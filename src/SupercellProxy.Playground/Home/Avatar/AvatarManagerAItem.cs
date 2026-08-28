using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c language="csharp">AvatarManagerAItem</c>.
/// </summary>
internal sealed record AvatarManagerAItem(
    int Unknown0,
    int Kind,
    int Unknown1,
    int? KindValue,
    int Unknown2
)
{
    internal static AvatarManagerAItem Decode(MessageStream stream)
    {
        var unknown0 = stream.ReadVarInt();
        var kind = stream.ReadVarInt();
        var unknown1 = stream.ReadVarInt();
        int? kindValue = kind is 1 ? stream.ReadVarInt() : null;
        return new AvatarManagerAItem(unknown0, kind, unknown1, kindValue, stream.ReadVarInt());
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Kind);
        stream.WriteVarInt(Unknown1);

        if (Kind is 1)
            stream.WriteVarInt(
                KindValue ?? throw new InvalidOperationException($"{nameof(KindValue)} is null.")
            );

        stream.WriteVarInt(Unknown2);
    }
}
