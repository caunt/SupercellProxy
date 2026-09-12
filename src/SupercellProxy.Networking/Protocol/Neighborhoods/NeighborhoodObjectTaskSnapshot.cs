namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Represents decoded <c language="csharp">NeighborhoodObjectTaskSnapshot</c> home data.
/// </summary>
public sealed record NeighborhoodObjectTaskSnapshot
{

    /// <summary>
    /// Gets or sets the <c language="csharp">PlayerLevelAtTaskStart</c> value.
    /// </summary>
    public int PlayerLevelAtTaskStart { get; init; }

    /// <summary>
    /// Gets the Seen value.
    /// </summary>
    public bool Seen { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TaskDataId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("TaskDataId")]
    public int TaskDataIdentifier { get; init; }
    /// <summary>
    /// Gets the Task State value.
    /// </summary>
    public int TaskState { get; init; }
}
