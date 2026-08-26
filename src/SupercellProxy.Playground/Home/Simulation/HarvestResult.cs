using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home.Simulation;

/// <summary>
/// Represents <c>HarvestResult</c>.
/// </summary>
/// <param name="FieldGlobalId">The <c>FieldGlobalId</c> value.</param>
/// <param name="Crop">The <c>Crop</c> value.</param>
/// <param name="CropCountBefore">The <c>CropCountBefore</c> value.</param>
/// <param name="CropCountAfter">The <c>CropCountAfter</c> value.</param>
/// <param name="ExperienceBefore">The <c>ExperienceBefore</c> value.</param>
/// <param name="ExperienceAfter">The <c>ExperienceAfter</c> value.</param>
/// <param name="FieldIsEmpty">The <c>FieldIsEmpty</c> value.</param>
/// <param name="GainSubTick">The <c>GainSubTick</c> value.</param>
/// <param name="CompletionSubTick">The <c>CompletionSubTick</c> value.</param>
/// <param name="SynchronizedSubTick">The <c>SynchronizedSubTick</c> value.</param>
public sealed record HarvestResult(
    int FieldGlobalId,
    DataTableReference Crop,
    int CropCountBefore,
    int CropCountAfter,
    int ExperienceBefore,
    int ExperienceAfter,
    bool FieldIsEmpty,
    int GainSubTick,
    int CompletionSubTick,
    int SynchronizedSubTick
);
