using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Defines the Upgrade Building Command contract.
/// </summary>
/// <summary>
/// Defines the Building Global Id contract.
/// </summary>
/// <summary>
/// Defines the Upgrade Option contract.
/// </summary>
public sealed record UpgradeBuildingCommand([property: System.Text.Json.Serialization.JsonPropertyName("BuildingGlobalId")] int BuildingGlobalId, int UpgradeOption) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.UpgradeBuildingCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static UpgradeBuildingCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int building = stream.ReadVarInt();
        int option = stream.ReadVarInt();

        return new UpgradeBuildingCommand(building, option);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(BuildingGlobalId);
        stream.WriteVarInt(UpgradeOption);
    }
}
