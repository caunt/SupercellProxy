using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>Stores a primitive literal in its declared type without boxing.</summary>
public readonly record struct LiteralValue
{
    private readonly long _integer;
    private readonly double _floatingPoint;
    private readonly string? _text;

    /// <summary>Initializes a new instance of the <see cref="LiteralValue"/> struct. Creates a 32-bit integer cell.</summary>
    /// <param name="value">The cell's number, as parsed from a column declared <c>int</c>; any <see cref="int"/> is accepted.</param>
    public LiteralValue(int value)
    {
        Kind = LiteralKind.Integer32;
        _integer = value;
    }

    /// <summary>Initializes a new instance of the <see cref="LiteralValue"/> struct. Creates a 64-bit integer cell.</summary>
    /// <param name="value">The cell's number, as parsed from a column declared <c>long</c>; it is kept apart from 32-bit cells, so
    /// <see cref="TryGetInt32"/> refuses it even when it fits.</param>
    public LiteralValue(long value)
    {
        Kind = LiteralKind.Integer64;
        _integer = value;
    }

    /// <summary>Initializes a new instance of the <see cref="LiteralValue"/> struct. Creates a single-precision floating-point cell.</summary>
    /// <param name="value">The cell's number, as parsed from a column declared <c>float</c>; it is stored widened to
    /// <see cref="double"/> and read back unchanged by <see cref="TryGetSingle"/>.</param>
    public LiteralValue(float value)
    {
        Kind = LiteralKind.SinglePrecision;
        _floatingPoint = value;
    }

    /// <summary>Initializes a new instance of the <see cref="LiteralValue"/> struct. Creates a double-precision floating-point cell.</summary>
    /// <param name="value">The cell's number, as parsed from a column declared <c>double</c>; only <see cref="TryGetDouble"/> reads it
    /// back.</param>
    public LiteralValue(double value)
    {
        Kind = LiteralKind.DoublePrecision;
        _floatingPoint = value;
    }

    /// <summary>Initializes a new instance of the <see cref="LiteralValue"/> struct. Creates a Boolean cell.</summary>
    /// <param name="value">The cell's flag, as parsed from a <c>Boolean</c> or <c>bool</c> column, where a cell of <c>1</c> or the word
    /// <see langword="true"/> in any letter case sets it.</param>
    public LiteralValue(bool value)
    {
        Kind = LiteralKind.Boolean;
        _integer = value ? 1 : 0;
    }

    /// <summary>Initializes a new instance of the <see cref="LiteralValue"/> struct. Creates a text cell.</summary>
    /// <param name="value">The cell's text exactly as it appears in the CSV after quote removal; an empty string is allowed.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>; use <see langword="default"/> for an
    /// empty cell instead.</exception>
    public LiteralValue(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Kind = LiteralKind.Text;
        _text = value;
    }

    /// <summary>Gets the declared type; the default value represents a <see langword="null"/> literal.</summary>
    /// <value>The type the constructor was called with, which the parser takes from the column's type row;
    /// <see cref="LiteralKind.Null"/> for a <see langword="default"/> instance, such as a cell left empty.</value>
    public LiteralKind Kind { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return Kind switch
        {
            LiteralKind.Null => string.Empty,
            LiteralKind.Integer32 or LiteralKind.Integer64 => _integer.ToString(CultureInfo.InvariantCulture),
            LiteralKind.SinglePrecision => float.CreateChecked(_floatingPoint).ToString(CultureInfo.InvariantCulture),
            LiteralKind.DoublePrecision => _floatingPoint.ToString(CultureInfo.InvariantCulture),
            LiteralKind.Boolean => (_integer != 0).ToString(),
            LiteralKind.Text => _text ?? string.Empty,
            _ => string.Empty,
        };
    }

    /// <summary>Reads a cell declared as a Boolean.</summary>
    /// <param name="value">Receives the flag, or <see langword="false"/> when the cell is of another kind.</param>
    /// <returns><see langword="true"/> if <see cref="Kind"/> is <see cref="LiteralKind.Boolean"/>, since numbers and text are never
    /// converted; otherwise, <see langword="false"/>.</returns>
    public bool TryGetBoolean(out bool value)
    {
        value = Kind == LiteralKind.Boolean && _integer != 0;

        return Kind == LiteralKind.Boolean;
    }

    /// <summary>Reads a cell declared as a double-precision floating-point number.</summary>
    /// <param name="value">Receives the number, or 0 when the cell is of another kind.</param>
    /// <returns><see langword="true"/> if <see cref="Kind"/> is exactly <see cref="LiteralKind.DoublePrecision"/>, so a
    /// <see cref="LiteralKind.SinglePrecision"/> or integer cell does not qualify; otherwise, <see langword="false"/>.</returns>
    public bool TryGetDouble(out double value)
    {
        value = Kind == LiteralKind.DoublePrecision ? _floatingPoint : default;

        return Kind == LiteralKind.DoublePrecision;
    }

    /// <summary>Reads a cell declared as a 32-bit integer.</summary>
    /// <param name="value">Receives the number, or 0 when the cell is of another kind.</param>
    /// <returns><see langword="true"/> if <see cref="Kind"/> is exactly <see cref="LiteralKind.Integer32"/>, so a
    /// <see cref="LiteralKind.Integer64"/> cell does not qualify even when its value would fit; otherwise,
    /// <see langword="false"/>.</returns>
    public bool TryGetInt32(out int value)
    {
        value = Kind == LiteralKind.Integer32 ? int.CreateChecked(_integer) : default;

        return Kind == LiteralKind.Integer32;
    }

    /// <summary>Reads a cell declared as a 64-bit integer.</summary>
    /// <param name="value">Receives the number, or 0 when the cell is of another kind.</param>
    /// <returns><see langword="true"/> if <see cref="Kind"/> is exactly <see cref="LiteralKind.Integer64"/>, so a 32-bit
    /// <see cref="LiteralKind.Integer32"/> cell does not qualify; otherwise, <see langword="false"/>.</returns>
    public bool TryGetInt64(out long value)
    {
        value = Kind == LiteralKind.Integer64 ? _integer : default;

        return Kind == LiteralKind.Integer64;
    }

    /// <summary>Reads a cell declared as a single-precision floating-point number.</summary>
    /// <param name="value">Receives the number, or 0 when the cell is of another kind.</param>
    /// <returns><see langword="true"/> if <see cref="Kind"/> is exactly <see cref="LiteralKind.SinglePrecision"/>, so a
    /// <see cref="LiteralKind.DoublePrecision"/> cell does not qualify; otherwise, <see langword="false"/>.</returns>
    public bool TryGetSingle(out float value)
    {
        value = Kind == LiteralKind.SinglePrecision ? float.CreateChecked(_floatingPoint) : default;

        return Kind == LiteralKind.SinglePrecision;
    }

    /// <summary>Reads a cell declared as text.</summary>
    /// <param name="value">Receives the text, or <see langword="null"/> when the cell is not text.</param>
    /// <returns><see langword="true"/> if <see cref="Kind"/> is <see cref="LiteralKind.Text"/>, which also covers columns of an
    /// undeclared type; otherwise, <see langword="false"/>.</returns>
    public bool TryGetString([NotNullWhen(true)] out string? value)
    {
        value = _text;

        return value is not null;
    }
}
