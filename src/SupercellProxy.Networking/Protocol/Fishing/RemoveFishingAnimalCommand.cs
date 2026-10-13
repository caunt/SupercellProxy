using System.Collections.Frozen;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Removes a departing duck or lobster from its processing facility.</summary>
public sealed record RemoveFishingAnimalCommand(int AnimalId, bool Duck)
    : Command
{
    /// <summary>The contract id when <see cref="Duck"/> is true.</summary>
    public const int RemoveDuckCommandType = 161;

    /// <summary>The contract id when <see cref="Duck"/> is false.</summary>
    public const int RemoveLobsterCommandType = 122;

    /// <summary>Gets the codec of each contract the record carries, by contract id.</summary>
    public static FrozenDictionary<int, CommandCodec<RemoveFishingAnimalCommand>> ContractCodecs { get; } = new Dictionary<int, CommandCodec<RemoveFishingAnimalCommand>>
    {
        [RemoveLobsterCommandType] = Create(RemoveLobsterCommandType, duck: false),
        [RemoveDuckCommandType] = Create(RemoveDuckCommandType, duck: true),
    }.ToFrozenDictionary();

    /// <inheritdoc />
    public override int Type => Duck ? RemoveDuckCommandType : RemoveLobsterCommandType;

    private static CommandCodec<RemoveFishingAnimalCommand> Create(int type, bool duck)
    {
        return Codecs.Command(
            type,
            Codecs.Composite(Codecs.VarInt, static command => command.AnimalId, (animalId) => new RemoveFishingAnimalCommand(animalId, duck))
        );
    }
}
