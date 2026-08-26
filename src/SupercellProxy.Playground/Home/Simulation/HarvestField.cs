using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home.Simulation;

/// <summary>
/// Represents <c>HarvestField</c>.
/// </summary>
/// <param name="FieldGlobalId">The <c>FieldGlobalId</c> value.</param>
/// <param name="Crop">The <c>Crop</c> value.</param>
/// <param name="HarvestCount">The <c>HarvestCount</c> value.</param>
/// <param name="ExperienceReward">The <c>ExperienceReward</c> value.</param>
public sealed record HarvestField(
    int FieldGlobalId,
    DataTableReference Crop,
    int HarvestCount,
    int ExperienceReward
);
