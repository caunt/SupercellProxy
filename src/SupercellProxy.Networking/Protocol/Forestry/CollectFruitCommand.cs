using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Collects one fruit from a fruit tree or berry bush.</summary>
public sealed record CollectFruitCommand(int FruitIndex, [property: System.Text.Json.Serialization.JsonPropertyName("FruitTreeGlobalId")] int FruitTreeGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CollectFruitCommandType;

    /// <summary>Decodes a value from the supplied protocol payload.</summary>
    public static CollectFruitCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new CollectFruitCommand(stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(FruitIndex);
        stream.WriteVarInt(FruitTreeGlobalId);
    }
}
