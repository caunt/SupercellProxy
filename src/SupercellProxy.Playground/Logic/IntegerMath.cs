namespace SupercellProxy.Playground.Logic;

/// <summary>
/// Represents <c>IntegerMath</c>.
/// </summary>
public static class IntegerMath
{
    private static readonly sbyte[] AngleTable =
    [
        0,
        0,
        1,
        1,
        2,
        2,
        3,
        3,
        4,
        4,
        4,
        5,
        5,
        6,
        6,
        7,
        7,
        8,
        8,
        8,
        9,
        9,
        10,
        10,
        11,
        11,
        11,
        12,
        12,
        13,
        13,
        14,
        14,
        14,
        15,
        15,
        16,
        16,
        17,
        17,
        17,
        18,
        18,
        19,
        19,
        19,
        20,
        20,
        21,
        21,
        21,
        22,
        22,
        22,
        23,
        23,
        24,
        24,
        24,
        25,
        25,
        25,
        26,
        26,
        27,
        27,
        27,
        28,
        28,
        28,
        29,
        29,
        29,
        30,
        30,
        30,
        31,
        31,
        31,
        32,
        32,
        32,
        33,
        33,
        33,
        34,
        34,
        34,
        35,
        35,
        35,
        35,
        36,
        36,
        36,
        37,
        37,
        37,
        37,
        38,
        38,
        38,
        39,
        39,
        39,
        39,
        40,
        40,
        40,
        40,
        41,
        41,
        41,
        41,
        42,
        42,
        42,
        42,
        43,
        43,
        43,
        43,
        44,
        44,
        44,
        44,
        45,
        45,
        45,
    ];

    private static readonly short[] SineTable =
    [
        0,
        18,
        36,
        54,
        71,
        89,
        107,
        125,
        143,
        160,
        178,
        195,
        213,
        230,
        248,
        265,
        282,
        299,
        316,
        333,
        350,
        367,
        384,
        400,
        416,
        433,
        449,
        465,
        481,
        496,
        512,
        527,
        543,
        558,
        573,
        587,
        602,
        616,
        630,
        644,
        658,
        672,
        685,
        698,
        711,
        724,
        737,
        749,
        761,
        773,
        784,
        796,
        807,
        818,
        828,
        839,
        849,
        859,
        868,
        878,
        887,
        896,
        904,
        912,
        920,
        928,
        935,
        943,
        949,
        956,
        962,
        968,
        974,
        979,
        984,
        989,
        994,
        998,
        1002,
        1005,
        1008,
        1011,
        1014,
        1016,
        1018,
        1020,
        1022,
        1023,
        1023,
        1024,
        1024,
    ];

    /// <summary>
    /// Gets <c>Sine</c>.
    /// </summary>
    public static int GetSine(int degrees)
    {
        var normalized = degrees % 360;

        if (normalized < 0)
            normalized += 360;

        var tableIndex = normalized > 179 ? normalized - 180 : normalized;
        tableIndex = tableIndex > 90 ? 180 - tableIndex : tableIndex;
        var value = SineTable[tableIndex];
        return normalized > 179 ? -value : value;
    }

    /// <summary>
    /// Gets <c>VectorAngle</c>.
    /// </summary>
    public static int GetVectorAngle(int x, int y)
    {
        if ((x | y) is 0)
            return 0;

        if (x >= 1 && y >= 0)
        {
            if (y < x)
                return AngleTable[(y << 7) / x];

            return 90 - AngleTable[(x << 7) / y];
        }

        var absoluteX = x < 0 ? -x : x;

        if (x <= 0 && y >= 1)
        {
            if (absoluteX < y)
                return AngleTable[(absoluteX << 7) / y] + 90;

            return 180 - AngleTable[(y << 7) / absoluteX];
        }

        var absoluteY = y < 0 ? -y : y;

        if (x < 0 && y <= 0)
        {
            if (absoluteY < absoluteX)
                return AngleTable[(absoluteY << 7) / absoluteX] + 180;

            return 270 - AngleTable[(absoluteX << 7) / absoluteY];
        }

        if (absoluteX < absoluteY)
            return AngleTable[(absoluteX << 7) / absoluteY] + 270;

        if (x is 0)
            return 0;

        var angle = 360 - AngleTable[(absoluteY << 7) / absoluteX];
        return angle >= 360 ? angle - 360 : angle;
    }

    /// <summary>
    /// Gets <c>AngleDifference</c>.
    /// </summary>
    public static int GetAngleDifference(int degrees)
    {
        var normalized = degrees % 360;

        if (normalized < 0)
            normalized += 360;

        return normalized > 179 ? normalized - 360 : normalized;
    }

    /// <summary>
    /// Gets <c>SquareRoot</c>.
    /// </summary>
    public static int GetSquareRoot(int value)
    {
        if (value < 0)
            return -1;

        var remainder = uint.CreateTruncating(value);
        var result = 0U;
        var bit = 1U << 30;

        while (bit > remainder)
            bit >>= 2;

        while (bit is not 0)
        {
            if (remainder >= result + bit)
            {
                remainder -= result + bit;
                result = (result >> 1) + bit;
            }
            else
            {
                result >>= 1;
            }

            bit >>= 2;
        }

        return int.CreateTruncating(result);
    }

    /// <summary>
    /// Calculates the native integer length of the vector formed by <paramref name="x"/> and <paramref name="y"/>.
    /// </summary>
    public static int GetVectorLength(int x, int y)
    {
        return GetSquareRoot(unchecked(x * x + y * y));
    }
}
