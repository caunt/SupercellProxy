namespace SupercellProxy.Playground.Logic;

/// <summary>
/// Represents <c>GameRandom</c>.
/// </summary>
/// <remarks>
/// Initializes a new <see cref="GameRandom"/> instance.
/// </remarks>
public sealed class GameRandom(int seed)
{
    /// <summary>
    /// Gets or sets the <c>State</c> value.
    /// </summary>
    public int State { get; private set; } = seed;

    /// <summary>
    /// Gets or sets the <c>Calls</c> value.
    /// </summary>
    public int Calls { get; private set; }

    /// <summary>
    /// Executes the <c>Reset</c> operation.
    /// </summary>
    public void Reset(int seed)
    {
        State = seed;
        Calls = 0;
    }

    /// <summary>
    /// Executes the <c>NextInt</c> operation.
    /// </summary>
    public int NextInt(int upperBound)
    {
        if (upperBound <= 0)
            return 0;

        Calls++;

        var state = unchecked(uint.CreateTruncating(State));

        if (state is 0)
            state = uint.MaxValue;

        state ^= state << 13;
        state ^= unchecked(uint.CreateTruncating((int.CreateTruncating(state) >> 17)));
        state ^= state << 5;
        State = unchecked(int.CreateTruncating(state));

        var value = int.CreateTruncating(state) < 0 ? unchecked(0U - state) : state;
        return unchecked(int.CreateTruncating((value % uint.CreateTruncating(upperBound))));
    }
}
