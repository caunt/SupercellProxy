namespace SupercellProxy.Networking.Protocol.MapGame.Fuel;

/// <summary>Represents retained Valley fuel-generator state.</summary>
public sealed record MapGameFuelManagerSnapshot
{
    /// <summary>Gets the active Valley sun-point task identifiers.</summary>
    public int[] CurrentTasks { get; init; } = [];

    /// <summary>Gets the active Valley sun-point task counters.</summary>
    public int[] Counters { get; init; } = [];

    /// <summary>Gets a value indicating whether a Farm Pass fuel spin is available.</summary>
    public bool FarmPassSpin { get; init; }

    /// <summary>Gets a value indicating whether a free fuel spin is available.</summary>
    public bool FreeSpin { get; init; }

    /// <summary>Gets a value indicating whether the current fuel prize was claimed.</summary>
    public bool FuelPrizeClaimed { get; init; }

    /// <summary>Gets the retained fuel random seed.</summary>
    public int FuelRandomSeed { get; init; }

    /// <summary>Gets the next Valley sun-point task identifiers.</summary>
    public int[] NextTasks { get; init; } = [];

    /// <summary>Gets the next day index at which Valley sun-point tasks may refresh.</summary>
    public int NextValidTasksDayIndex { get; init; }

    /// <summary>Gets the retained purchased fuel-spin count.</summary>
    public int NumberOfPurchasedSpins { get; init; }

    /// <summary>Gets the retained fuel-spin count.</summary>
    public int NumberSpins { get; init; }

    /// <summary>Gets the retained randomized fuel-prize index.</summary>
    public int RandomizedFuelIndex { get; init; }

    /// <summary>Gets the retained randomized fuel prizes.</summary>
    public int[] RandomizedFuelPrizes { get; init; } = [];

    /// <summary>Gets the retained Valley sun-point level.</summary>
    public int SunPointsLevel { get; init; }

    /// <summary>Gets the acknowledged Valley sun-point amount.</summary>
    public int SunPointsSeen { get; init; }
}
