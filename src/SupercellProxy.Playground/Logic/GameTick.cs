namespace SupercellProxy.Playground.Logic;

/// <summary>
/// Represents <c>GameTick</c>.
/// </summary>
public sealed class GameTick
{
    /// <summary>
    /// Defines the <c>InitialUpdateCount</c> value.
    /// </summary>
    public const int InitialUpdateCount = 2;

    /// <summary>
    /// Defines the <c>UpdatesPerSecond</c> value.
    /// </summary>
    public const int UpdatesPerSecond = 30;

    /// <summary>
    /// Gets or sets the <c>SubTick</c> value.
    /// </summary>
    public int SubTick { get; private set; }

    /// <summary>
    /// Gets or sets the <c>Tick</c> value.
    /// </summary>
    public int Tick { get; private set; }

    /// <summary>
    /// Advances <c>GameTick</c> state.
    /// </summary>
    public void Advance()
    {
        var subTick = SubTick;
        SubTick = unchecked(subTick + 1);

        if ((subTick & 1) is not 0)
            Tick = unchecked(Tick + 1);
    }

    /// <summary>
    /// Advances <c>GameTick</c> state.
    /// </summary>
    public void Advance(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        for (var i = 0; i < count; i++)
            Advance();
    }
}
