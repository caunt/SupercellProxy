namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>Identifies the literal or preserved structured value in a table change.</summary>
public enum TableCellValueKind
{
    /// <summary>No value was supplied.</summary>
    Missing,
    /// <summary>The change clears the cell.</summary>
    Null,
    /// <summary>The change contains text.</summary>
    Text,
    /// <summary>The change contains a numeric literal.</summary>
    Number,
    /// <summary>The change contains a boolean literal.</summary>
    Boolean,
    /// <summary>The change contains a structured value whose schema is not decoded.</summary>
    Structured,
}
