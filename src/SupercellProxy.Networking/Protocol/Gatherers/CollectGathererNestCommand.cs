using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Gatherers;

/// <summary>Collects the completed product from a gatherer nest.</summary>
public sealed record CollectGathererNestCommand([property: System.Text.Json.Serialization.JsonPropertyName("GathererNestGlobalId")] int GathererNestGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CollectGathererNestCommandType;

    /// <summary>Decodes a value from the supplied protocol payload.</summary>
    public static CollectGathererNestCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int id = stream.ReadVarInt();

        return new CollectGathererNestCommand(id);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(GathererNestGlobalId);
    }
}
