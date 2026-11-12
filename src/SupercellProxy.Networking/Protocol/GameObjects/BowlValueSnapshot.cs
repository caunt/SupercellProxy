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

    /// <summary>Gets the integer value, with boolean <see langword="true"/> represented as one.</summary>
    /// <value>For a bowl saved as a number, that number, which pet bowls read as a food count; for a bowl saved as a JSON boolean, 1 for a set flag and 0 for a cleared one.</value>
    public int IntegerValue { get; }

    /// <summary>Gets whether the saved value used a JSON boolean.</summary>
    /// <value><see langword="true"/> if the save wrote the bowl as a JSON boolean, a form that writing it back keeps; otherwise, <see langword="false"/>.</value>
    /// <remarks>A bowl saved as a number has the value <see langword="false"/>.</remarks>
    public bool BooleanRepresentation { get; }
}
