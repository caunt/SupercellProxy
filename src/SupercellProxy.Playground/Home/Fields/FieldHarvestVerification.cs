using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed record FieldHarvestVerification(
    int FieldGlobalId,
    int FieldPositionX,
    int FieldPositionY,
    DataTableReference Crop,
    int HarvestCount,
    int ExperienceReward,
    int CropCountBefore,
    int ExperienceBefore,
    int GainSubTick,
    int CompletionSubTick,
    int SynchronizedSubTick
);
