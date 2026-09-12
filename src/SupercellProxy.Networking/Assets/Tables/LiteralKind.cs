namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>Identifies the declared primitive type of a literal value.</summary>
public enum LiteralKind
{
    /// <summary>A null or absent value.</summary>
    Null,
    /// <summary>A signed 32-bit integer.</summary>
    Integer,
    /// <summary>A signed 64-bit integer.</summary>
    LongInteger,
    /// <summary>A single-precision floating-point number.</summary>
    Single,
    /// <summary>A double-precision floating-point number.</summary>
    Double,
    /// <summary>A Boolean value.</summary>
    Boolean,
    /// <summary>A text value.</summary>
    String,
}
