using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Json;

/// <summary>A boolean protocol flag that retains its boolean or integer wire representation.</summary>
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(ProtocolFlagConverter))]
public readonly record struct ProtocolFlag
{
    internal ProtocolFlag(ProtocolFlagKind kind, int integer)
    {
        Kind = kind;
        Integer = integer;
    }

    /// <summary>Gets the retained integer value.</summary>
    /// <value>The number as read for an integer flag; 1 for a set and 0 for a cleared JSON boolean; 0 when the flag was <see langword="null"/> or absent.</value>
    public int Integer { get; }

    /// <summary>Gets the original wire representation.</summary>
    /// <value>Whether the flag was absent, <see langword="null"/>, a JSON boolean or an integer; it decides how the flag is written back, and is <see cref="ProtocolFlagKind.Missing"/> for a default instance.</value>
    public ProtocolFlagKind Kind { get; }

    /// <summary>Gets the decoded flag value.</summary>
    /// <value><see langword="true"/> if <see cref="Integer"/> is non-zero, so a flag that was <see langword="null"/> or absent never qualifies; otherwise, <see langword="false"/>.</value>
    public bool Value => Integer is not 0;
}
