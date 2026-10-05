using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Marks an exhausted plant for clearing, optionally buying its missing tool with coins.</summary>
public sealed record StartPlantClearingCommand(int PlantId, bool BuyMissingTool) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.StartPlantClearingCommandType;

    /// <summary>Decodes the native plant id and coin-purchase flag.</summary>
    public static StartPlantClearingCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int id = stream.ReadVarInt();
        bool buy = stream.ReadBoolean();

        return new(id, buy);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(PlantId);
        stream.WriteBoolean(BuyMissingTool);
    }
}
