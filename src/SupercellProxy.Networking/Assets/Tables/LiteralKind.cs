namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>Identifies the declared primitive type of a literal value.</summary>
public enum LiteralKind
{
    /// <summary>A <see langword="null"/> or absent value.</summary>
    Null,
    /// <summary>A signed 32-bit integer, from a column declared <c>int</c>.</summary>
    Integer32,
    /// <summary>A signed 64-bit integer, from a column declared <c>long</c>.</summary>
    Integer64,
    /// <summary>A single-precision floating-point number, from a column declared <c>float</c>.</summary>
    SinglePrecision,
    /// <summary>A double-precision floating-point number, from a column declared <c>double</c>.</summary>
    DoublePrecision,
    /// <summary>A <see langword="true"/> or <see langword="false"/> flag, from a column declared <c>Boolean</c> or <c>bool</c>.</summary>
    Boolean,
    /// <summary>
    /// The cell's characters kept as written, from a column declared <c>String</c> or of a type the parser does not recognize; an empty cell stays
    /// an empty string.
    /// </summary>
    Text,
}
