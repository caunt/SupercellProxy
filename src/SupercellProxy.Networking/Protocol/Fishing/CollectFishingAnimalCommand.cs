using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Collects and removes one ready lobster or duck through its native command.</summary>
public sealed record CollectFishingAnimalCommand(int AnimalId, bool Duck) : Command
{
    /// <inheritdoc />
    public override int Type => Duck ? CommandRegistry.CollectDuckCommandType : CommandRegistry.CollectLobsterCommandType;
    /// <summary>Decodes a collection command for the selected native animal type.</summary>
    public static CollectFishingAnimalCommand Decode(MessageStream stream, CommandEnvironment environment, bool duck)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int id = stream.ReadVarInt();

        return new(id, duck);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(AnimalId);
    }
}
