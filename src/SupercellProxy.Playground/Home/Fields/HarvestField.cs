using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c language="csharp">HarvestField</c>.
/// </summary>
/// <param name="FieldGlobalId">The <c language="csharp">FieldGlobalId</c> value.</param>
/// <param name="Crop">The <c language="csharp">Crop</c> value.</param>
/// <param name="HarvestCount">The <c language="csharp">HarvestCount</c> value.</param>
/// <param name="ExperienceReward">The <c language="csharp">ExperienceReward</c> value.</param>
internal sealed record HarvestField(
    int FieldGlobalId,
    DataTableReference Crop,
    int HarvestCount,
    int ExperienceReward
);
