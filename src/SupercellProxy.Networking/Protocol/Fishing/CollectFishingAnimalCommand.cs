using System.Collections.Frozen;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Collects and removes one ready lobster or duck through its native command.</summary>
public sealed record CollectFishingAnimalCommand(int AnimalId, bool Duck)
    : Command
{
    /// <summary>The contract id when <see cref="Duck"/> is true.</summary>
    public const int CollectDuckCommandType = 159;

    /// <summary>The contract id when <see cref="Duck"/> is false.</summary>
    public const int CollectLobsterCommandType = 121;

    /// <summary>Gets the codec of each contract the record carries, by contract id.</summary>
    public static FrozenDictionary<int, CommandCodec<CollectFishingAnimalCommand>> ContractCodecs { get; } = new Dictionary<int, CommandCodec<CollectFishingAnimalCommand>>
    {
        [CollectLobsterCommandType] = Create(CollectLobsterCommandType, duck: false),
        [CollectDuckCommandType] = Create(CollectDuckCommandType, duck: true),
    }.ToFrozenDictionary();

    /// <inheritdoc />
    public override int Type => Duck ? CollectDuckCommandType : CollectLobsterCommandType;

    private static CommandCodec<CollectFishingAnimalCommand> Create(int type, bool duck)
    {
        return Codecs.Command(
            type,
            Codecs.Composite(Codecs.VarInt, static command => command.AnimalId, (animalId) => new CollectFishingAnimalCommand(animalId, duck))
        );
    }
}
