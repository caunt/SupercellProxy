namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>The native Valley commands that carry the shared map-game task structure, named by the events native creates for them.</summary>
public enum MapGameTaskKind
{
    /// <summary>Completes a personal dump task (DumpTaskCompleted).</summary>
    CompleteDumpTask,

    /// <summary>Joins a shared task a neighbour found (SharedTaskJoined), collecting its node emoji.</summary>
    JoinSharedTask,

    /// <summary>Completes a personal delivery task (DumpTaskCompleted).</summary>
    CompleteDeliveryTask,

    /// <summary>Skips a task's replacement cooldown (TaskCooldownSkipped and TaskRemoved).</summary>
    SkipTaskCooldown,

    /// <summary>Completes an obstacle by paying (ObstaclePaidCompletion), removing its TruckHelpRequest emoji.</summary>
    CompleteObstacleWithPayment,

    /// <summary>Asks for help with an obstacle (ObstacleHelpRequested), adding the TruckHelpRequest emoji.</summary>
    RequestObstacleHelp,

    /// <summary>Completes an obstacle (ObstacleCompleted), removing its TruckHelpRequest emoji.</summary>
    CompleteObstacle,

    /// <summary>Marks a road-stop task's chicken found, adding the RoadStopChickenFound emoji that collecting it removes.</summary>
    FindChicken,

    /// <summary>Collects a found chicken (ChickenTaskCollected).</summary>
    CollectChicken,

    /// <summary>Marks a shared task's chicken found, adding the SocialTaskChickenFound emoji that joining the task collects.</summary>
    FindSharedTaskChicken,

    /// <summary>Collects a sanctuary animal (SanctuaryAnimalCollected).</summary>
    CollectSanctuaryAnimal,

    /// <summary>Collects a gas station (GasStationCollected).</summary>
    CollectGasStation,

    /// <summary>Purchases a gas station (GasStationPurchased).</summary>
    PurchaseGasStation,
}
