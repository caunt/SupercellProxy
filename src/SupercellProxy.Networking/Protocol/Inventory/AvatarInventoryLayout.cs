using System.Collections.ObjectModel;

using SupercellProxy.Networking.Assets.Tables;

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

    /// <summary>Gets the inventory layout used by the 1.72 client.</summary>
    /// <value>A shared instance with the 1.72 unlock tables and three value maps; <see cref="For"/> also returns it when the version is unknown.</value>
    public static AvatarInventoryLayout Legacy { get; } = new(LegacyUnlockTableIds, mapCount: 3);

    /// <summary>Gets the 1.73 layout, which adds mollusc goods, mollusc beds, the supply shed, and Angus's store.</summary>
    /// <value>A shared instance with the <see cref="Legacy"/> unlock tables plus tables 353, 354 and the supply shed table, and four value maps.</value>
    public static AvatarInventoryLayout WithAngus { get; } = new(unlockTableIds: [.. LegacyUnlockTableIds, 353, 354, DataTableRegistry.SupplyShedTableId], mapCount: 4);

    /// <summary>Gets the number of integer arrays in the inventory section, which includes the ordered unlock bitsets and the final shop-badge list.</summary>
    /// <value>One more than the number of <see cref="UnlockTableIds"/>; the decoder reads exactly this many arrays, and encoding a section with another count fails.</value>
    public int ArrayCount => _unlockTableIds.Length + 1;

    /// <summary>Gets the primary and helper inventory map count.</summary>
    /// <value>How many data-id to value maps the inventory section carries, read right after the arrays: 3 in the 1.72 layout and 4 in the 1.73 layout.</value>
    public int MapCount { get; }

    /// <summary>Gets the array containing data ids with new-item shop badges.</summary>
    /// <value>The index of the shop-badge array, which comes right after the last unlock bitset and so equals the number of unlock tables.</value>
    public int ShopBadgeArrayIndex => _unlockTableIds.Length;

    /// <summary>Gets the table ids in native ascending serialization order.</summary>
    /// <value>A read-only, ascending list of data table ids; the bitset at array index <c>i</c> holds the unlocked rows of the table at position <c>i</c>.</value>
    public ReadOnlyCollection<int> UnlockTableIds { get; }

    /// <summary>Selects a native layout from the connection's full client version.</summary>
    /// <param name="version">The game version the connection reported; only major 1 with minor 72 or 73 is supported, and <see langword="null"/> selects the 1.72 layout.</param>
    /// <returns><see cref="Legacy"/> for 1.72 or an unknown version, or <see cref="WithAngus"/> for 1.73.</returns>
    /// <exception cref="NotSupportedException"><paramref name="version"/> is set and is neither 1.72 nor 1.73.</exception>
    public static AvatarInventoryLayout For(Version? version)
    {
        return version is null || version is { Major: 1, Minor: 72 }
            ? Legacy
            : version is { Major: 1, Minor: 73 } ? WithAngus
            : throw new NotSupportedException($"The avatar inventory layout for game version {version} is not implemented.");
    }

    /// <summary>Finds the native unlock bitmap for a data table.</summary>
    /// <param name="tableId">Id of the data table whose rows can be unlocked, such as the table of a product or decoration.</param>
    /// <returns>The index of the table's unlock bitset in the inventory arrays, or a negative number when the layout keeps no unlock bits for that table.</returns>
    public int FindUnlockIndex(int tableId)
    {
        return Array.BinarySearch(_unlockTableIds, tableId);
    }
}
