using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">MapGameTaskStatePayload</c>.
/// </summary>
internal abstract record MapGameTaskStatePayload
{
    internal abstract void Encode(MessageStream stream);

    internal static MapGameTaskStatePayload Decode(string taskType, MessageStream stream)
    {
        return taskType switch
        {
            "Dump" => MapGameDumpTaskStatePayload.Decode(stream),
            "Delivery" => MapGameDeliveryTaskStatePayload.Decode(stream),
            "Obstacle" => MapGameObstacleTaskStatePayload.Decode(stream),
            "Chicken" => MapGameChickenTaskStatePayload.Decode(stream),
            "Sanctuary Animal" => MapGameSanctuaryAnimalTaskStatePayload.Decode(stream),
            "Gas Station" => MapGameGasStationTaskStatePayload.Decode(stream),
            "Offload Sanctuary Animal" => MapGameOffloadSanctuaryAnimalTaskStatePayload.Decode(
                stream
            ),
            _ => throw new NotSupportedException(
                $"Map-game task type '{taskType}' is not supported."
            ),
        };
    }
}
