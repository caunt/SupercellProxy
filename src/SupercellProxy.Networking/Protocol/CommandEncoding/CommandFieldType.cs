namespace SupercellProxy.Networking.Protocol.CommandEncoding;

/// <summary>
/// Defines the Command Field Type contract.
/// </summary>
public enum CommandFieldType
{
    /// <summary>
    /// Identifies the Var Int wire value.
    /// </summary>
    [System.Text.Json.Serialization.JsonStringEnumMemberName("VarInt")]
    VarInt,

    /// <summary>
    /// Identifies the Var Long wire value.
    /// </summary>
    [System.Text.Json.Serialization.JsonStringEnumMemberName("VarLong")]
    VarLong,

    /// <summary>
    /// Identifies the Int32 wire value.
    /// </summary>
    Int32,

    /// <summary>
    /// Identifies the Byte wire value.
    /// </summary>
    Byte,

    /// <summary>
    /// Identifies the UInt16 wire value.
    /// </summary>
    UInt16,

    /// <summary>
    /// Identifies the Boolean wire value.
    /// </summary>
    Boolean,

    /// <summary>
    /// Identifies the String wire value.
    /// </summary>
    String,

    /// <summary>
    /// Identifies the Long Id wire value.
    /// </summary>
    [System.Text.Json.Serialization.JsonStringEnumMemberName("LongId")]
    LongId,

    /// <summary>
    /// Identifies the Optional Long Id wire value.
    /// </summary>
    [System.Text.Json.Serialization.JsonStringEnumMemberName("OptionalLongId")]
    OptionalLongId,

    /// <summary>
    /// Identifies the Data Reference wire value.
    /// </summary>
    DataReference,

    /// <summary>
    /// Identifies the Byte Array wire value.
    /// </summary>
    ByteArray,

    /// <summary>
    /// Identifies the Var Int Array wire value.
    /// </summary>
    [System.Text.Json.Serialization.JsonStringEnumMemberName("VarIntArray")]
    VarIntArray,

    /// <summary>
    /// Identifies the Var Long Array wire value.
    /// </summary>
    [System.Text.Json.Serialization.JsonStringEnumMemberName("VarLongArray")]
    VarLongArray,

    /// <summary>
    /// Identifies the Nullable Var Long Array wire value.
    /// </summary>
    [System.Text.Json.Serialization.JsonStringEnumMemberName("NullableVarLongArray")]
    NullableVarLongArray,

    /// <summary>
    /// Identifies the Var Int Pair Array wire value.
    /// </summary>
    [System.Text.Json.Serialization.JsonStringEnumMemberName("VarIntPairArray")]
    VarIntPairArray,

    /// <summary>
    /// Identifies the Data Reference Var Int Pair Array wire value.
    /// </summary>
    [System.Text.Json.Serialization.JsonStringEnumMemberName("DataReferenceVarIntPairArray")]
    DataReferenceVarIntPairArray,

    /// <summary>
    /// Identifies the Data Reference Array wire value.
    /// </summary>
    DataReferenceArray,

    /// <summary>
    /// Identifies the String Array wire value.
    /// </summary>
    StringArray,

    /// <summary>
    /// Identifies the Byte Counted Var Int Array wire value.
    /// </summary>
    [System.Text.Json.Serialization.JsonStringEnumMemberName("ByteCountedVarIntArray")]
    ByteCountedVarIntArray,

    /// <summary>
    /// Identifies the Optional Int32 String wire value.
    /// </summary>
    OptionalInt32String,

    /// <summary>
    /// Identifies the Optional Structure wire value.
    /// </summary>
    OptionalStructure,

    /// <summary>
    /// Identifies the Structure Array wire value.
    /// </summary>
    StructureArray,
}
