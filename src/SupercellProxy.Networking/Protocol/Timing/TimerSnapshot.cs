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
    /// Gets the <c language="csharp">IsComplete</c> value.
    /// </summary>
    [JsonIgnore]
    public bool IsComplete => TicksLeft < 1;
}
