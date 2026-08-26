using System.Globalization;
using System.Text.Json;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed class FieldState
{
    private readonly DataTableReference emptyFieldData;
    private readonly DataTableResolver dataTableResolver;

    private FieldState(
        GameObjectState gameObject,
        DataTableReference emptyFieldData,
        DataTableResolver dataTableResolver
    )
    {
        GameObject = gameObject;
        this.emptyFieldData = emptyFieldData;
        this.dataTableResolver = dataTableResolver;
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
    public DataTableReference Data => GameObject.Data;
    public TimerSnapshot GrowthTimer { get; private set; }
    public bool IsEmpty => Data.GlobalId == emptyFieldData.GlobalId;
    public bool HasGrowthTimer { get; private set; }
    public int HarvestCount { get; private set; }
    public int ExperienceReward { get; private set; }
    public int InstantCompleteCost { get; private set; }
    public bool IsHarvestReady { get; private set; }
    public bool IsHarvestStarted { get; private set; }
    public bool IsHarvestGainCompleted { get; private set; }

    public static FieldState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver
    )
    {
        const string fieldsFile = "data/fields.csv";

        if (!dataTableResolver.TryGetTableId(fieldsFile, out var fieldTableId))
            throw new InvalidOperationException(
                $"{fieldsFile} is not registered as a native data table."
            );

        if (
            !dataTableResolver.TryResolve(
                fieldTableId * DataTableResolver.GlobalIdTableSize,
                out var emptyFieldData
            ) || emptyFieldData.Name is not "EmptyField"
        )
        {
            throw new InvalidDataException($"{fieldsFile} does not start with EmptyField.");
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

        if (IsHarvestStarted || IsHarvestGainCompleted)
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {GlobalId} has already started harvesting."
                )
            );

        IsHarvestStarted = true;
    }

    internal void CompleteGain()
    {
        if (IsEmpty)
            throw new InvalidOperationException(
                string.Create(CultureInfo.InvariantCulture, $"Field {GlobalId} is empty.")
            );

        if (IsHarvestGainCompleted)
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

        IsHarvestGainCompleted = true;
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

    internal void CompleteHarvest()
    {
        if (IsEmpty)
            throw new InvalidOperationException(
                string.Create(CultureInfo.InvariantCulture, $"Field {GlobalId} is empty.")
            );

        if (!IsHarvestGainCompleted)
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {GlobalId} has not completed its harvest gain."
                )
            );

        GameObject.ChangeData(emptyFieldData, dataTableResolver);
        HasGrowthTimer = false;
        GrowthTimer = default;
        HarvestCount = 0;
        ExperienceReward = 0;
        InstantCompleteCost = 0;
        IsHarvestReady = false;
        IsHarvestStarted = false;
        IsHarvestGainCompleted = false;
    }

    internal FieldState CreateEmptyReplacement(int globalId)
    {
        if (!IsHarvestGainCompleted)
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Field {GlobalId} has not completed its harvest gain."
                )
            );

        var snapshot = GameObject.Snapshot with
        {
            DataGlobalId = emptyFieldData.GlobalId,
            Timer = default,
        };
        var dimensions = GameObjectDimensionsResolver.Resolve(emptyFieldData, dataTableResolver);
        var gameObject = new GameObjectState(
            globalId,
            snapshot,
            emptyFieldData,
            dimensions.Width,
            dimensions.Height
        );
        return new FieldState(gameObject, emptyFieldData, dataTableResolver);
    }

    private int ResolveCropValue(string fieldName)
    {
        if (IsEmpty)
            return 0;

        if (!dataTableResolver.TryResolveInt(Data.GlobalId, fieldName, out var value))
            throw new InvalidDataException($"Field data {Data.Name} has no {fieldName} value.");

        return value;
    }
}
