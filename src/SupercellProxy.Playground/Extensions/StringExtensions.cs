namespace SupercellProxy.Playground.Extensions;

/// <summary>
/// Represents <c>StringExtensions</c>.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Executes the <c>ToStringPadLeft</c> operation.
    /// </summary>
    public static string? ToStringPadLeft<T>(this T value, int width, char @char = '.')
        where T : struct
    {
        return value.ToString()?.PadLeft(width, @char);
    }
}
