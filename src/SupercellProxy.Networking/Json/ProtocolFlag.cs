using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Json;

/// <summary>A boolean protocol flag that retains its boolean or integer wire representation.</summary>
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(ProtocolFlagConverter))]
public readonly record struct ProtocolFlag
{
    internal ProtocolFlag(ProtocolFlagKind kind, int number)
    {
        Kind = kind;
        Number = number;
    }

    /// <summary>Gets the retained number.</summary>
    /// <value>The number as read for a numeric flag; 1 for a set and 0 for a cleared JSON boolean; 0 when the flag was <see langword="null"/> or absent.</value>
    public int Number { get; }

    /// <summary>Gets the original wire representation.</summary>
    /// <value>Whether the flag was absent, <see langword="null"/>, a JSON boolean or a JSON number; it decides how the flag is written back, and is <see cref="ProtocolFlagKind.Missing"/> for a default instance.</value>
    public ProtocolFlagKind Kind { get; }

    /// <summary>Gets the decoded flag value.</summary>
    /// <value><see langword="true"/> if <see cref="Number"/> is non-zero, so a flag that was <see langword="null"/> or absent never qualifies; otherwise, <see langword="false"/>.</value>
    public bool Value => Number is not 0;
}
