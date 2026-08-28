using System.Runtime.InteropServices;
using System.Text.Json;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">TimerSnapshot</c> home data.
/// </summary>
/// <param name="StartSeconds">The timer start timestamp in seconds.</param>
/// <param name="TicksLeft">The number of timer ticks remaining.</param>
[StructLayout(LayoutKind.Auto)]
internal readonly record struct TimerSnapshot(int StartSeconds, int TicksLeft)
{
    /// <summary>
    /// Decodes a <c language="csharp">TimerSnapshot</c> from native wire data.
    /// </summary>
    public static TimerSnapshot Decode(JsonElement value)
    {
        if (value.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
            return default;

        if (value.ValueKind is not JsonValueKind.Object)
            throw new InvalidDataException($"Unsupported timer JSON kind: {value.ValueKind}.");

        return new TimerSnapshot(
            ReadInt32(value, nameof(StartSeconds)),
            ReadInt32(value, nameof(TicksLeft))
        );
    }

    /// <summary>
    /// Gets the <c language="csharp">IsComplete</c> value.
    /// </summary>
    public bool IsComplete => TicksLeft < 1;

    private static int ReadInt32(JsonElement value, string propertyName)
    {
        return value.TryGetProperty(propertyName, out var property) ? property.GetInt32() : 0;
    }
}
