namespace SupercellProxy.Playground.Logic;

/// <summary>
/// Represents <c language="csharp">GameRandom</c>.
/// </summary>
/// <remarks>
/// Initializes a new <see cref="GameRandom"/> instance.
/// </remarks>
internal sealed class GameRandom(int seed)
{
    /// <summary>
    /// Gets or sets the <c language="csharp">State</c> value.
    /// </summary>
    public int State { get; private set; } = seed;

    /// <summary>
    /// Gets or sets the <c language="csharp">Calls</c> value.
    /// </summary>
    public int Calls { get; private set; }

    internal Action<int, int>? NextIntObserved { get; set; }

    /// <summary>
    /// Executes the <c language="csharp">Reset</c> operation.
    /// </summary>
    public void Reset(int seed)
    {
        State = seed;
        Calls = 0;
    }

    /// <summary>
    /// Executes the <c language="csharp">NextInt</c> operation.
    /// </summary>
    public int NextInt(int upperBound)
    {
        if (upperBound <= 0)
        {
            NextIntObserved?.Invoke(upperBound, 0);
            return 0;
        }

        Calls++;

        var state = unchecked(uint.CreateTruncating(State));

        if (state is 0)
            state = uint.MaxValue;

        state ^= state << 13;
        state ^= unchecked(uint.CreateTruncating((int.CreateTruncating(state) >> 17)));
        state ^= state << 5;
        State = unchecked(int.CreateTruncating(state));

        var value = int.CreateTruncating(state) < 0 ? unchecked(0U - state) : state;
        var result = unchecked(int.CreateTruncating((value % uint.CreateTruncating(upperBound))));
        NextIntObserved?.Invoke(upperBound, result);
        return result;
    }
}
