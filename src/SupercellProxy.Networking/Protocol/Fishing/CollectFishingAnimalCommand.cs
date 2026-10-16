using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Collects and removes one ready lobster or duck through its native command.</summary>
public sealed record CollectFishingAnimalCommand(int AnimalId, bool Duck)
    : Command
{
    /// <summary>Gets the contract of the native command that collects a duck.</summary>
    public static CommandCodec<CollectFishingAnimalCommand> DuckCodec { get; } = Create(name: "CollectDuckCommand", duck: true);

    /// <summary>Gets the contract of the native command that collects a lobster.</summary>
    public static CommandCodec<CollectFishingAnimalCommand> LobsterCodec { get; } = Create(name: "CollectLobsterCommand", duck: false);

    /// <inheritdoc />
    public override CommandContract Contract => Duck ? DuckCodec : LobsterCodec;

    private static CommandCodec<CollectFishingAnimalCommand> Create(string name, bool duck)
    {
        return Codecs.Command(
            name,
            Codecs.Composite(Codecs.VarInt, static command => command.AnimalId, animalId => new CollectFishingAnimalCommand(animalId, duck))
        );
    }
}
