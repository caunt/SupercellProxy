using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Commands;

internal enum MapGameEventFieldType
{
    VarInt,
    Boolean,
    Byte,
    LongId,
    OptionalLongId,
    DataReference,
    OptionalPawn,
    OptionalTask,
    OptionalTaskCollection,
    OptionalVarIntArray,
    OptionalState,
    OptionalDumpTaskState,
    OptionalProfileData,
}
