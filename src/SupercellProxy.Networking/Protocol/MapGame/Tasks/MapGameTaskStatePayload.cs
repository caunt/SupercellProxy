using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// Represents <c language="csharp">MapGameTaskStatePayload</c>.
/// </summary>
public abstract record MapGameTaskStatePayload
{

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameTaskStatePayload Decode(string taskType, MessageStream stream)
    {
        return taskType switch
        {
            "Dump" => MapGameDumpTaskStatePayload.Decode(stream),
            "Delivery" => MapGameDeliveryTaskStatePayload.Decode(stream),
            "Obstacle" => MapGameObstacleTaskStatePayload.Decode(stream),
            "Chicken" => MapGameChickenTaskStatePayload.Decode(stream),
            "Sanctuary Animal" => MapGameSanctuaryAnimalTaskStatePayload.Decode(stream),
            "Gas Station" => MapGameGasStationTaskStatePayload.Decode(stream),
            "Offload Sanctuary Animal" => MapGameOffloadSanctuaryAnimalTaskStatePayload.Decode(stream),
            _ => throw new NotSupportedException($"Map-game task type '{taskType}' is not supported."),
        };
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public abstract void Encode(MessageStream stream);
}
