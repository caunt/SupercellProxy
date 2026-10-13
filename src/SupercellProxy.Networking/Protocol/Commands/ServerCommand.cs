namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>
/// <para>Base wire representation shared by commands issued by the server.</para>
/// </summary>
public abstract record ServerCommand : Command
{
    private int _serverCommandId = -1;

    /// <summary>Gets the server's sequence number for the command.</summary>
    [System.Text.Json.Serialization.JsonPropertyName("ServerCommandId")]
    public int ServerCommandId
    {
        get => _serverCommandId;
        init => _serverCommandId = value;
    }

    internal void ApplyServerCommandId(int serverCommandId)
    {
        _serverCommandId = serverCommandId;
    }
}
