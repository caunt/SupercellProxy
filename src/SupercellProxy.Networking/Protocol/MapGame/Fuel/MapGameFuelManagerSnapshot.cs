namespace SupercellProxy.Networking.Protocol.MapGame.Fuel;

/// <summary>Represents retained Valley fuel-generator state.</summary>
public sealed record MapGameFuelManagerSnapshot
{
    /// <summary>Gets or sets the active Valley sun-point task ids.</summary>
    /// <value>Global ids of today's sun-point task rows, paired by index with <see cref="Counters"/>. Empty by default.</value>
    public int[] CurrentTasks { get; init; } = [];

    /// <summary>Gets or sets the active Valley sun-point task counters.</summary>
    /// <value>Non-negative progress per task in <see cref="CurrentTasks"/>, in the same order and of the same length.</value>
    public int[] Counters { get; init; } = [];

    /// <summary>Gets or sets a value indicating whether a Farm Pass fuel spin is available.</summary>
    /// <value><see langword="true"/> if the Farm Pass <c>ExtraFuelSpin</c> perk gave a spin today that is still unused; otherwise, <see langword="false"/>.</value>
    public bool FarmPassSpin { get; init; }

    /// <summary>Gets or sets a value indicating whether a free fuel spin is available.</summary>
    /// <value><see langword="true"/> if the day's free spin is unused; otherwise, <see langword="false"/>.</value>
    public bool FreeSpin { get; init; }

    /// <summary>Gets or sets a value indicating whether the current fuel prize was claimed.</summary>
    /// <value><see langword="true"/> if the prize the last spin landed on has been collected; otherwise, <see langword="false"/>.</value>
    public bool FuelPrizeClaimed { get; init; }

    /// <summary>Gets or sets the retained fuel random seed.</summary>
    /// <value>A random generator state saved with the fuel wheel.</value>
    public int FuelRandomSeed { get; init; }

    /// <summary>Gets or sets the next Valley sun-point task ids.</summary>
    /// <value>Task row global ids prepared for the day in <see cref="NextValidTasksDayIndex"/>. Empty by default.</value>
    public int[] NextTasks { get; init; } = [];

    /// <summary>Gets or sets the next day index at which Valley sun-point tasks may refresh.</summary>
    /// <value>A UTC day number (days since 1970-01-01) one past the day of the last reset.</value>
    public int NextValidTasksDayIndex { get; init; }

    /// <summary>Gets or sets the retained purchased fuel-spin count.</summary>
    /// <value>How many spins the player has bought since the last daily reset.</value>
    public int NumberOfPurchasedSpins { get; init; }

    /// <summary>Gets or sets the retained fuel-spin count.</summary>
    /// <value>A running total of claimed fuel prizes.</value>
    public int NumberSpins { get; init; }

    /// <summary>Gets or sets the retained randomized fuel-prize index.</summary>
    /// <value>The zero-based slot of <see cref="RandomizedFuelPrizes"/> the last spin landed on.</value>
    public int RandomizedFuelIndex { get; init; }

    /// <summary>Gets or sets the retained randomized fuel prizes.</summary>
    /// <value>The fuel amounts on the wheel, normally four, sorted without repeats.</value>
    public int[] RandomizedFuelPrizes { get; init; } = [];

    /// <summary>Gets or sets the retained Valley sun-point level.</summary>
    /// <value>The sun-point progression level whose <c>RequiredSunPointsToFillBar</c> the next bar needs.</value>
    public int SunPointsLevel { get; init; }

    /// <summary>Gets or sets the acknowledged Valley sun-point amount.</summary>
    /// <value>The sun-point total the player last saw.</value>
    public int SunPointsSeen { get; init; }
}
