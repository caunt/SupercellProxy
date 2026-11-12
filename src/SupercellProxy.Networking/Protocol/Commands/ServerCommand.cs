using System.Text.Json.Serialization;
namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>
/// <para>Base wire representation shared by commands issued by the server.</para>
/// </summary>
public abstract record ServerCommand : Command
{
    private int _serverCommandId = -1;

    /// <summary>Gets or sets the server's sequence number for the command.</summary>
    /// <value>The id written first in the clientbound metadata; -1 until it is set or decoded, and a decoded value of -1 is rejected as invalid.</value>
    [JsonPropertyName("ServerCommandId")]
    public int ServerCommandId
    {
        get => _serverCommandId;
        init => _serverCommandId = value;
    }

    internal void Apply(int serverCommandId)
    {
        _serverCommandId = serverCommandId;
    }
}
