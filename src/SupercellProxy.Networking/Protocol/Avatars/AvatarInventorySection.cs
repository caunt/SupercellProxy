using SupercellProxy.Networking.Protocol.Inventory;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>The avatar inventory block, whose array and map counts follow the game version's inventory layout.</summary>
internal readonly record struct AvatarInventorySection(AvatarInventoryLayout Layout, int[][] Values, DataReferenceValue[][] Maps, int DeprecatedDataCount, int Unknown0)
    : ICodecProvider<AvatarInventorySection>
{
    /// <inheritdoc />
    public static ICodec<AvatarInventorySection> Codec { get; } = Codecs.Create(
        static stream =>
        {
            AvatarInventoryLayout layout = AvatarInventoryLayout.ForVersion(stream.GameVersion);
            int[][] values = new int[layout.ArrayCount][];

            for (int index = 0; index < values.Length; index++)
                values[index] = ValuesCodec.Read(stream);

            DataReferenceValue[][] maps = new DataReferenceValue[layout.MapCount][];

            for (int index = 0; index < maps.Length; index++)
                maps[index] = MapCodec.Read(stream);

            int deprecatedDataCount = stream.ReadVarInt();

            return deprecatedDataCount is not 0
                ? throw new InvalidDataException(message: "The deprecated polymorphic inventory section is not implemented.")
                : new AvatarInventorySection(layout, values, maps, deprecatedDataCount, stream.ReadVarInt());
        },
        static (stream, section) =>
        {
            if (section.Values.Length != section.Layout.ArrayCount || section.Maps.Length != section.Layout.MapCount)
                throw new InvalidOperationException(message: "Unexpected inventory field count.");

            if (section.DeprecatedDataCount is not 0)
                throw new InvalidOperationException(message: "Cannot encode an unsupported avatar section.");

            foreach (int[] values in section.Values)
                ValuesCodec.Write(stream, values);

            foreach (DataReferenceValue[] values in section.Maps)
                MapCodec.Write(stream, values);

            stream.WriteVarInt(section.DeprecatedDataCount);
            stream.WriteVarInt(section.Unknown0);
        }
    );

    private static ICodec<DataReferenceValue[]> MapCodec => field ??= Codecs.Array(DataReferenceValue.Codec, CollectionCount.Bounded);

    private static ICodec<int[]> ValuesCodec => field ??= Codecs.Array(Codecs.VarInt, CollectionCount.Bounded);
}
