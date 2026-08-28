namespace SupercellProxy.Playground.Commands;

internal enum CommandFieldType
{
    VarInt,
    VarLong,
    Int32,
    Byte,
    UInt16,
    Boolean,
    String,
    LongId,
    OptionalLongId,
    DataReference,
    ByteArray,
    VarIntArray,
    VarLongArray,
    NullableVarLongArray,
    VarIntPairArray,
    DataReferenceVarIntPairArray,
    DataReferenceArray,
    StringArray,
    ByteCountedVarIntArray,
    OptionalInt32String,
    OptionalStructure,
    StructureArray,
}
