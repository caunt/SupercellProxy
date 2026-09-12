using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>Stores a primitive literal in its declared type without boxing.</summary>
public readonly record struct LiteralValue
{
    private readonly long _integer;
    private readonly double _floatingPoint;
    private readonly string? _text;

    /// <summary>Creates a 32-bit integer cell.</summary>
    public LiteralValue(int value)
    {
        Kind = LiteralKind.Integer;
        _integer = value;
    }

    /// <summary>Creates a 64-bit integer cell.</summary>
    public LiteralValue(long value)
    {
        Kind = LiteralKind.LongInteger;
        _integer = value;
    }

    /// <summary>Creates a single-precision floating-point cell.</summary>
    public LiteralValue(float value)
    {
        Kind = LiteralKind.Single;
        _floatingPoint = value;
    }

    /// <summary>Creates a double-precision floating-point cell.</summary>
    public LiteralValue(double value)
    {
        Kind = LiteralKind.Double;
        _floatingPoint = value;
    }

    /// <summary>Creates a Boolean cell.</summary>
    public LiteralValue(bool value)
    {
        Kind = LiteralKind.Boolean;
        _integer = value ? 1 : 0;
    }

    /// <summary>Creates a text cell.</summary>
    public LiteralValue(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Kind = LiteralKind.String;
        _text = value;
    }

    /// <summary>Gets the declared type; the default value represents a null literal.</summary>
    public LiteralKind Kind { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return Kind switch
        {
            LiteralKind.Null => string.Empty,
            LiteralKind.Integer or LiteralKind.LongInteger => _integer.ToString(CultureInfo.InvariantCulture),
            LiteralKind.Single => float.CreateChecked(_floatingPoint).ToString(CultureInfo.InvariantCulture),
            LiteralKind.Double => _floatingPoint.ToString(CultureInfo.InvariantCulture),
            LiteralKind.Boolean => (_integer != 0).ToString(),
            LiteralKind.String => _text ?? string.Empty,
            _ => string.Empty,
        };
    }

    /// <summary>Reads a cell declared as a Boolean.</summary>
    public bool TryGetBoolean(out bool value)
    {
        value = Kind == LiteralKind.Boolean && _integer != 0;

        return Kind == LiteralKind.Boolean;
    }

    /// <summary>Reads a cell declared as a double-precision floating-point number.</summary>
    public bool TryGetDouble(out double value)
    {
        value = Kind == LiteralKind.Double ? _floatingPoint : default;

        return Kind == LiteralKind.Double;
    }

    /// <summary>Reads a cell declared as a 32-bit integer.</summary>
    public bool TryGetInt32(out int value)
    {
        value = Kind == LiteralKind.Integer ? int.CreateChecked(_integer) : default;

        return Kind == LiteralKind.Integer;
    }

    /// <summary>Reads a cell declared as a 64-bit integer.</summary>
    public bool TryGetInt64(out long value)
    {
        value = Kind == LiteralKind.LongInteger ? _integer : default;

        return Kind == LiteralKind.LongInteger;
    }

    /// <summary>Reads a cell declared as a single-precision floating-point number.</summary>
    public bool TryGetSingle(out float value)
    {
        value = Kind == LiteralKind.Single ? float.CreateChecked(_floatingPoint) : default;

        return Kind == LiteralKind.Single;
    }

    /// <summary>Reads a cell declared as text.</summary>
    public bool TryGetString([NotNullWhen(true)] out string? value)
    {
        value = _text;

        return value is not null;
    }
}
