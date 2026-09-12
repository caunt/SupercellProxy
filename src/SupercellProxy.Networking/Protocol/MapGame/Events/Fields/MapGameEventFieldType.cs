namespace SupercellProxy.Networking.Protocol.MapGame.Events.Fields;

/// <summary>
/// Defines the Map Game Event Field Type contract.
/// </summary>
public enum MapGameEventFieldType
{
    /// <summary>
    /// Identifies the Var Int wire value.
    /// </summary>
    [System.Text.Json.Serialization.JsonStringEnumMemberName("VarInt")]
    VariableInt,

    /// <summary>
    /// Identifies the Boolean wire value.
    /// </summary>
    Boolean,

    /// <summary>
    /// Identifies the Byte wire value.
    /// </summary>
    Byte,

    /// <summary>
    /// Identifies the Long Id wire value.
    /// </summary>
    [System.Text.Json.Serialization.JsonStringEnumMemberName("LongId")]
    LongIdentifier,

    /// <summary>
    /// Identifies the Optional Long Id wire value.
    /// </summary>
    [System.Text.Json.Serialization.JsonStringEnumMemberName("OptionalLongId")]
    OptionalLongIdentifier,

    /// <summary>
    /// Identifies the Data Reference wire value.
    /// </summary>
    DataReference,

    /// <summary>
    /// Identifies the Optional Pawn wire value.
    /// </summary>
    OptionalPawn,

    /// <summary>
    /// Identifies the Optional Task wire value.
    /// </summary>
    OptionalTask,

    /// <summary>
    /// Identifies the Optional Task Collection wire value.
    /// </summary>
    OptionalTaskCollection,

    /// <summary>
    /// Identifies the Optional Var Int Array wire value.
    /// </summary>
    [System.Text.Json.Serialization.JsonStringEnumMemberName("OptionalVarIntArray")]
    OptionalVariableIntArray,

    /// <summary>
    /// Identifies the Optional State wire value.
    /// </summary>
    OptionalState,

    /// <summary>
    /// Identifies the Optional Dump Task State wire value.
    /// </summary>
    OptionalDumpTaskState,

    /// <summary>
    /// Identifies the Optional Profile Data wire value.
    /// </summary>
    OptionalProfileData,
}
