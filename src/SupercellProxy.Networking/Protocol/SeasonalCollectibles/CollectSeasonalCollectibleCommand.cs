using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.SeasonalCollectibles;

/// <summary>Collects the reward from a spawned seasonal gift.</summary>
public sealed record CollectSeasonalCollectibleCommand(int CollectibleId) : Command
{
    /// <summary>Gets the native command type.</summary>
    public override int Type => CommandRegistry.CollectSeasonalCollectibleCommandType;

    /// <summary>Decodes the collectible instance id.</summary>
    public static CollectSeasonalCollectibleCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt());
    }

    /// <summary>Encodes the collectible instance id.</summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(CollectibleId);
    }
}
