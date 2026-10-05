namespace SupercellProxy.Networking.Protocol.CommandEncoding;

/// <summary>
/// <para>Base wire representation shared by commands issued by the server.</para>
/// </summary>
public abstract record ServerCommand : Command
{
    /// <summary>
    /// Gets the <c language="csharp">ServerCommandId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("ServerCommandId")]
    public int ServerCommandId { get; init; } = -1;
}
