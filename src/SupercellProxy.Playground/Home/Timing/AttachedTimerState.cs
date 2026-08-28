using System.Text.Json;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class AttachedTimerState
{
    private AttachedTimerState(bool active, int startSeconds, int ticksLeft)
    {
        Active = active;
        StartSeconds = startSeconds;
        TicksLeft = ticksLeft;
    }

    public bool Active { get; }
    public int StartSeconds { get; private set; }
    public int TicksLeft { get; private set; }

    public static AttachedTimerState Create(bool active, JsonElement snapshot)
    {
        if (snapshot.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
            return new AttachedTimerState(active, startSeconds: 0, ticksLeft: 0);

        if (snapshot.ValueKind is not JsonValueKind.Object)
            throw new InvalidDataException("The attached timer snapshot is invalid.");

        return new AttachedTimerState(
            active,
            ReadNumber(snapshot, nameof(StartSeconds)),
            ReadNumber(snapshot, nameof(TicksLeft))
        );
    }

    public void Advance(int updateCount)
    {
        if (!Active || TicksLeft < 1)
            return;

        TicksLeft = Math.Max(TicksLeft - updateCount, 0);
    }

    public void SetStartSeconds(int startSeconds)
    {
        StartSeconds = startSeconds;
        TicksLeft = checked(startSeconds * GameTick.TimerUpdatesPerSecond);
    }

    public int GetRemainingSeconds()
    {
        return TicksLeft / GameTick.TimerUpdatesPerSecond
            + (TicksLeft % GameTick.TimerUpdatesPerSecond is 0 ? 0 : 1);
    }

    private static int ReadNumber(JsonElement snapshot, string name)
    {
        if (
            !snapshot.TryGetProperty(name, out var value)
            || value.ValueKind is not JsonValueKind.Number
        )
        {
            return 0;
        }

        return value.TryGetInt32(out var integer)
            ? integer
            : int.CreateTruncating(value.GetDouble());
    }
}
