using System.Collections.Immutable;
using System.Runtime.InteropServices;

using SupercellProxy.Networking.Protocol.Inventory;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>The avatar inventory block, whose array and map counts follow the game version's inventory layout.</summary>
internal readonly record struct AvatarInventorySection(AvatarInventoryLayout Layout,
    ImmutableArray<ImmutableArray<int>> Values,
    ImmutableArray<ImmutableArray<DataReferenceValue>> Maps,
    int DeprecatedDataCount,
    int Unknown0)
    : ICodecProvider<AvatarInventorySection>
{
    /// <inheritdoc />
    public static ICodec<AvatarInventorySection> Codec { get; } = Codecs.Create(static stream =>
    {
        AvatarInventoryLayout layout = AvatarInventoryLayout.For(stream.GameVersion);
        ImmutableArray<int>[] values = new ImmutableArray<int>[layout.ArrayCount];

        for (int index = 0; index < values.Length; index++)
            values[index] = ValuesCodec.Read(stream);

        ImmutableArray<DataReferenceValue>[] maps = new ImmutableArray<DataReferenceValue>[layout.MapCount];

        for (int index = 0; index < maps.Length; index++)
            maps[index] = MapCodec.Read(stream);

        int deprecatedDataCount = stream.ReadVarInt();

        return deprecatedDataCount is not 0
            ? throw new InvalidDataException(message: "The deprecated polymorphic inventory section is not implemented.")
            : new AvatarInventorySection(layout,
                ImmutableCollectionsMarshal.AsImmutableArray(values),
                ImmutableCollectionsMarshal.AsImmutableArray(maps),
                deprecatedDataCount,
                stream.ReadVarInt());
    },
        static (stream, section) =>
        {
            if (section.Values.Length != section.Layout.ArrayCount || section.Maps.Length != section.Layout.MapCount)
                throw new InvalidOperationException(message: "Unexpected inventory field count.");

            if (section.DeprecatedDataCount is not 0)
                throw new InvalidOperationException(message: "Cannot encode an unsupported avatar section.");

            foreach (ImmutableArray<int> values in section.Values)
                ValuesCodec.Write(stream, values);

            foreach (ImmutableArray<DataReferenceValue> values in section.Maps)
                MapCodec.Write(stream, values);

            stream.WriteVarInt(section.DeprecatedDataCount);
            stream.WriteVarInt(section.Unknown0);
        });

    private static ICodec<ImmutableArray<DataReferenceValue>> MapCodec => field ??= Codecs.Array(DataReferenceValue.Codec, CollectionCount.Bounded);
    private static ICodec<ImmutableArray<int>> ValuesCodec => field ??= Codecs.Array(Codecs.VarInt, CollectionCount.Bounded);
}
