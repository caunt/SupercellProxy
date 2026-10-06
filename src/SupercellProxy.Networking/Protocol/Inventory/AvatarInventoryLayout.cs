using System.Collections.ObjectModel;

namespace SupercellProxy.Networking.Protocol.Inventory;

/// <summary>Defines the inventory collections encoded by supported native game releases.</summary>
public sealed class AvatarInventoryLayout
{
    private static readonly int[] LegacyUnlockTableIds =
    [
        3,
        4,
        6,
        13,
        15,
        16,
        17,
        22,
        23,
        24,
        25,
        26,
        27,
        28,
        29,
        30,
        33,
        36,
        38,
        44,
        48,
        51,
        60,
        61,
        64,
        65,
        66,
        68,
        69,
        75,
        77,
        78,
        80,
        82,
        84,
        101,
        105,
        109,
        115,
        117,
        118,
        119,
        120,
        121,
        129,
        133,
        134,
        145,
        148,
        149,
        150,
        152,
        153,
        166,
        167,
        168,
        169,
        170,
        176,
        177,
        184,
        185,
        187,
        202,
        236,
        238,
        239,
        240,
        241,
        247,
        248,
        253,
        261,
        262,
        263,
        264,
        265,
        266,
        267,
        268,
        269,
        270,
        274,
        278,
        279,
        280,
        281,
        286,
        294,
        311,
        334,
        345,
    ];

    private readonly int[] _unlockTableIds;

    private AvatarInventoryLayout(int[] unlockTableIds, int mapCount)
    {
        _unlockTableIds = unlockTableIds;
        UnlockTableIds = Array.AsReadOnly(unlockTableIds);
        MapCount = mapCount;
    }

    /// <summary>The inventory layout used by the 1.72 client.</summary>
    public static AvatarInventoryLayout Legacy { get; } = new(LegacyUnlockTableIds, mapCount: 3);

    /// <summary>The 1.73 layout adds mollusc goods, mollusc beds, the supply shed, and Angus's store.</summary>
    public static AvatarInventoryLayout WithAngus { get; } = new([.. LegacyUnlockTableIds, 353, 354, Assets.Tables.DataTableRegistry.SupplyShedTableId], mapCount: 4);

    /// <summary>Includes the ordered unlock bitsets and the final special-value list.</summary>
    public int ArrayCount => _unlockTableIds.Length + 1;

    /// <summary>Gets the primary and helper inventory map count.</summary>
    public int MapCount { get; }

    /// <summary>Gets the table ids in native ascending serialization order.</summary>
    public ReadOnlyCollection<int> UnlockTableIds { get; }

    /// <summary>Selects a native layout from the connection's full client version.</summary>
    public static AvatarInventoryLayout ForVersion(Version? version)
    {
        return version is null || version is { Major: 1, Minor: 72 }
            ? Legacy
            : version is { Major: 1, Minor: 73 } ? WithAngus
            : throw new NotSupportedException($"The avatar inventory layout for game version {version} is not implemented.");
    }

    /// <summary>Finds the native unlock bitmap for a data table.</summary>
    public int FindUnlockIndex(int tableId)
    {
        return Array.BinarySearch(_unlockTableIds, tableId);
    }
}
