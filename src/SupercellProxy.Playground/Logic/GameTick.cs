namespace SupercellProxy.Playground.Logic;

/// <summary>
/// Represents <c language="csharp">GameTick</c>.
/// </summary>
internal sealed class GameTick
{
    /// <summary>
    /// Defines the <c language="csharp">InitialUpdateCount</c> value.
    /// </summary>
    public const int InitialUpdateCount = 2;

    /// <summary>
    /// Defines the <c language="csharp">UpdatesPerSecond</c> value.
    /// </summary>
    public const int UpdatesPerSecond = 30;

    /// Defines the truncated duration of one update in milliseconds.
    public const int UpdateMilliseconds = 33;

    /// Defines the update rate used by native timer state.
    public const int TimerUpdatesPerSecond = 15;

    /// <summary>
    /// Gets or sets the <c language="csharp">SubTick</c> value.
    /// </summary>
    public int SubTick { get; private set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Tick</c> value.
    /// </summary>
    public int Tick { get; private set; }

    /// <summary>
    /// Advances <c language="csharp">GameTick</c> state.
    /// </summary>
    public void Advance()
    {
        var subTick = SubTick;
        SubTick = unchecked(subTick + 1);

        if ((subTick & 1) is not 0)
            Tick = unchecked(Tick + 1);
    }

    /// <summary>
    /// Advances <c language="csharp">GameTick</c> state.
    /// </summary>
    public void Advance(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        for (var i = 0; i < count; i++)
            Advance();
    }
}
