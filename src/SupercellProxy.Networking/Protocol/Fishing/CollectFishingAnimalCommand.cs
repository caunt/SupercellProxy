using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Collects and removes one ready lobster or duck through its native command.</summary>
public sealed record CollectFishingAnimalCommand(int AnimalIdentifier, bool Duck, int ExecutionPhaseCounter = -1, CommandData? DebugData0 = null, CommandData? DebugData1 = null) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => Duck ? CommandRegistry.CollectDuckCommandType : CommandRegistry.CollectLobsterCommandType;
    /// <summary>Decodes a collection command for the selected native animal type.</summary>
    public static CollectFishingAnimalCommand Decode(MessageStream stream, CommandEnvironment environment, bool duck)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int identifier = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new(identifier, duck, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(AnimalIdentifier);
        EncodeCommand(stream, environment);
    }
}
