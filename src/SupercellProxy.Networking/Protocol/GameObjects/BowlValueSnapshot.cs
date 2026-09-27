using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>A saved bowl value that retains its boolean or integer JSON representation.</summary>
[JsonConverter(typeof(BowlValueSnapshotConverter))]
public readonly record struct BowlValueSnapshot
{
    internal BowlValueSnapshot(int integerValue, bool booleanRepresentation)
    {
        IntegerValue = integerValue;
        BooleanRepresentation = booleanRepresentation;
    }

    /// <summary>Gets the integer value, with boolean true represented as one.</summary>
    public int IntegerValue { get; }

    /// <summary>Gets whether the saved value used a JSON boolean.</summary>
    public bool BooleanRepresentation { get; }
}
