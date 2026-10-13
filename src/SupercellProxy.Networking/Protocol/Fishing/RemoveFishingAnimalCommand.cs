using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Removes a departing duck or lobster from its processing facility.</summary>
public sealed record RemoveFishingAnimalCommand(int AnimalId, bool Duck)
    : Command
{
    /// <summary>Gets the contract of the native command that removes a duck.</summary>
    public static CommandCodec<RemoveFishingAnimalCommand> DuckCodec { get; } = Create(name: "RemoveDuckCommand", duck: true);

    /// <summary>Gets the contract of the native command that removes a lobster.</summary>
    public static CommandCodec<RemoveFishingAnimalCommand> LobsterCodec { get; } = Create(name: "RemoveLobsterCommand", duck: false);

    /// <inheritdoc />
    public override CommandContract Contract => Duck ? DuckCodec : LobsterCodec;

    private static CommandCodec<RemoveFishingAnimalCommand> Create(string name, bool duck)
    {
        return Codecs.Command(
            name,
            Codecs.Composite(Codecs.VarInt, static command => command.AnimalId, (animalId) => new RemoveFishingAnimalCommand(animalId, duck))
        );
    }
}
