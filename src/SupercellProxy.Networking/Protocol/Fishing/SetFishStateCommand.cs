using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Moves one fishing-area fish to the selected runtime state.</summary>
public sealed record SetFishStateCommand([property: System.Text.Json.Serialization.JsonPropertyName("FishGlobalId")] int FishGlobalId, int State) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.SetFishStateCommandType;

    /// <summary>Decodes a value from the supplied protocol payload.</summary>
    public static SetFishStateCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int fishGlobalId = stream.ReadVarInt();
        int state = stream.ReadVarInt();

        return new SetFishStateCommand(fishGlobalId, state);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(FishGlobalId);
        stream.WriteVarInt(State);
    }
}
