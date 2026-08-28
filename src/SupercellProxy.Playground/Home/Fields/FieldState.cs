using System.Globalization;
using System.Text.Json;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed class FieldState
{
    private readonly DataTableReference _emptyFieldData;
    private readonly DataTableResolver _dataTableResolver;

    private FieldState(
        GameObjectState gameObject,
        DataTableReference emptyFieldData,
        DataTableResolver dataTableResolver
    )
    {
        GameObject = gameObject;
        this._emptyFieldData = emptyFieldData;
        this._dataTableResolver = dataTableResolver;
        HasGrowthTimer =
            gameObject.Snapshot.Timer.ValueKind
                is not JsonValueKind.Undefined
                    and not JsonValueKind.Null;
        GrowthTimer = TimerSnapshot.Decode(gameObject.Snapshot.Timer);
        HarvestCount = ResolveCropValue("Harvest");
        ExperienceReward = ResolveCropValue("ExpCollect");
        InstantCompleteCost = ResolveCropValue("InstantComplete");
        IsHarvestReady = !IsEmpty && GrowthTimer.IsComplete;
    }

    public int GlobalId => GameObject.GlobalId;
    public GameObjectState GameObject { get; }
    public DataTableReference CropData => GameObject.Data;
    public DataTableReference GameplayData =>
        IsHarvestStarted || IsHarvestGainApplied ? _emptyFieldData : CropData;
    public TimerSnapshot GrowthTimer { get; private set; }
    public bool IsEmpty => CropData.GlobalId == _emptyFieldData.GlobalId;
    public bool HasGrowthTimer { get; private set; }
    public int HarvestCount { get; private set; }
    public int ExperienceReward { get; private set; }
    public int InstantCompleteCost { get; private set; }
    public bool IsHarvestReady { get; private set; }
    public bool IsHarvestStarted { get; private set; }
    public bool IsHarvestGainApplied { get; private set; }

    public static FieldState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver
    )
    {
        if (!dataTableResolver.TryGetTableId(GameAssetFiles.Fields, out var fieldTableId))
            throw new InvalidOperationException(
                $"{GameAssetFiles.Fields} is not registered as a native data table."
            );

        if (
            !dataTableResolver.TryResolve(
                fieldTableId * DataTableResolver.GlobalIdTableSize,
                out var emptyFieldData
            ) || emptyFieldData.Name is not "EmptyField"
        )
        {
            throw new InvalidDataException(
                $"{GameAssetFiles.Fields} does not start with EmptyField."
            );
        }

        return gameObjects
            .Where(gameObject => gameObject.Data.TableId == fieldTableId)
            .Select(gameObject => new FieldState(gameObject, emptyFieldData, dataTableResolver))
            .ToArray();
    }

    internal void StartHarvest()
    {
        if (IsEmpty)
            throw new InvalidOperationException(
                string.Create(CultureInfo.InvariantCulture, $"Field {GlobalId} is empty.")
            );

        if (!IsHarvestReady)
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {GlobalId} is not ready to harvest."
                )
            );

        if (IsHarvestStarted || IsHarvestGainApplied)
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {GlobalId} has already started harvesting."
                )
            );

        IsHarvestStarted = true;
    }

    internal void ApplyHarvestGain()
    {
        if (IsEmpty)
            throw new InvalidOperationException(
                string.Create(CultureInfo.InvariantCulture, $"Field {GlobalId} is empty.")
            );

        if (IsHarvestGainApplied)
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {GlobalId} has already completed its harvest gain."
                )
            );

        if (!IsHarvestReady)
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {GlobalId} is not ready to harvest."
                )
            );

        if (!IsHarvestStarted)
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {GlobalId} has not started harvesting."
                )
            );

        IsHarvestGainApplied = true;
    }

    internal void AdvanceSubTick()
    {
        if (!HasGrowthTimer)
            return;

        var ticksLeft = GrowthTimer.TicksLeft;

        if (ticksLeft > 0)
            ticksLeft--;

        GrowthTimer = new TimerSnapshot(ticksLeft < 1 ? 0 : GrowthTimer.StartSeconds, ticksLeft);
        IsHarvestReady = !IsEmpty && GrowthTimer.IsComplete;
    }

    internal FieldState CreateEmptyReplacement(int globalId)
    {
        if (!IsHarvestGainApplied)
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {GlobalId} has not completed its harvest gain."
                )
            );

        var snapshot = GameObject.Snapshot with
        {
            DataGlobalId = _emptyFieldData.GlobalId,
            Timer = default,
        };
        var dimensions = GameObjectDimensionsResolver.Resolve(_emptyFieldData, _dataTableResolver);
        var gameObject = new GameObjectState(
            globalId,
            snapshot,
            _emptyFieldData,
            dimensions.Width,
            dimensions.Height
        );
        return new FieldState(gameObject, _emptyFieldData, _dataTableResolver);
    }

    private int ResolveCropValue(string fieldName)
    {
        if (IsEmpty)
            return 0;

        if (!_dataTableResolver.TryResolveInt(CropData.GlobalId, fieldName, out var value))
            throw new InvalidDataException($"Field data {CropData.Name} has no {fieldName} value.");

        return value;
    }
}
