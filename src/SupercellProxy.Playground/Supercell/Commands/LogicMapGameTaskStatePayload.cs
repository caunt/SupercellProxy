using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

public abstract record LogicMapGameTaskStatePayload
{
    internal abstract void Encode(SupercellStream stream);

    internal static LogicMapGameTaskStatePayload Decode(string taskType, SupercellStream stream)
    {
        return taskType switch
        {
            "Dump" => LogicMapGameDumpTaskStatePayload.Decode(stream),
            "Delivery" => LogicMapGameDeliveryTaskStatePayload.Decode(stream),
            "Obstacle" => LogicMapGameObstacleTaskStatePayload.Decode(stream),
            "Chicken" => LogicMapGameChickenTaskStatePayload.Decode(stream),
            "Sanctuary Animal" => LogicMapGameSanctuaryAnimalTaskStatePayload.Decode(stream),
            "Gas Station" => LogicMapGameGasStationTaskStatePayload.Decode(stream),
            "Offload Sanctuary Animal" => LogicMapGameOffloadSanctuaryAnimalTaskStatePayload.Decode(stream),
            _ => throw new NotSupportedException($"Map-game task type '{taskType}' is not supported.")
        };
    }
}
