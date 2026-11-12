using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Timing;

/// <summary>
/// Represents decoded <c language="csharp">TimerSnapshot</c> home data.
/// </summary>
/// <param name="StartSeconds">The timer start timestamp in seconds.</param>
/// <param name="TicksLeft">The number of timer ticks remaining.</param>
[StructLayout(LayoutKind.Auto)]
public readonly record struct TimerSnapshot(int StartSeconds, int TicksLeft)
{
    /// <summary>
    /// Gets whether the countdown has run out (no ticks left).
    /// </summary>
    /// <value><see langword="true"/> if <see cref="TicksLeft"/> is 0 or negative; otherwise, <see langword="false"/>.</value>
    /// <remarks>The value is computed from the ticks and never written to JSON.</remarks>
    [JsonIgnore]
    public bool IsComplete => TicksLeft < 1;
}
