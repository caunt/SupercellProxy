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
    public int Integer { get; }

    /// <summary>Gets the original wire representation.</summary>
    public ProtocolFlagKind Kind { get; }

    /// <summary>Gets the decoded flag value.</summary>
    public bool Value => Integer is not 0;
}
