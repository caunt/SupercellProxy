namespace SupercellProxy.Networking.Json;

/// <summary>Identifies a flag's original JSON representation.</summary>
public enum ProtocolFlagKind
{
    /// <summary>The property was absent.</summary>
    Missing,
    /// <summary>The property explicitly contained null.</summary>
    Null,
    /// <summary>The property contained a JSON boolean.</summary>
    Boolean,
    /// <summary>The property contained an integer flag.</summary>
    Integer,
}
