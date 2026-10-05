using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Collects an already cut plant's rewards and removes the plant.</summary>
public sealed record CompletePlantClearingCommand(int PlantId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CompletePlantClearingCommandType;

    /// <summary>Decodes the native plant id.</summary>
    public static CompletePlantClearingCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int id = stream.ReadVarInt();

        return new(id);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(PlantId);
    }
}
