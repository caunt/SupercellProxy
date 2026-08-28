using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c language="csharp">HarvestResult</c>.
/// </summary>
/// <param name="FieldGlobalId">The <c language="csharp">FieldGlobalId</c> value.</param>
/// <param name="Crop">The <c language="csharp">Crop</c> value.</param>
/// <param name="CropCountBefore">The <c language="csharp">CropCountBefore</c> value.</param>
/// <param name="CropCountAfter">The <c language="csharp">CropCountAfter</c> value.</param>
/// <param name="ExperienceBefore">The <c language="csharp">ExperienceBefore</c> value.</param>
/// <param name="ExperienceAfter">The <c language="csharp">ExperienceAfter</c> value.</param>
/// <param name="FieldIsEmpty">The <c language="csharp">FieldIsEmpty</c> value.</param>
/// <param name="GainSubTick">The <c language="csharp">GainSubTick</c> value.</param>
/// <param name="CompletionSubTick">The <c language="csharp">CompletionSubTick</c> value.</param>
/// <param name="SynchronizedSubTick">The <c language="csharp">SynchronizedSubTick</c> value.</param>
internal sealed record HarvestResult(
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
