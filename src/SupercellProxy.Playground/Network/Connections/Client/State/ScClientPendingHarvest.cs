using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Network.Connections.Client;

internal sealed record ScClientPendingHarvest(
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
