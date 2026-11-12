using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Removes a departing duck or lobster from its processing facility.</summary>
/// <param name="AnimalId">Game object global id of the departing duck or lobster; it is the only field written on the wire.</param>
/// <param name="Duck"><see langword="true"/> for a duck, sent as the native <c>RemoveDuckCommand</c>; <see langword="false"/> for a lobster, sent as <c>RemoveLobsterCommand</c>. It picks the codec and is not itself encoded.</param>
public sealed record RemoveFishingAnimalCommand(int AnimalId, bool Duck)
    : Command
{
    /// <summary>Gets the contract of the native command that removes a duck.</summary>
    /// <value>The codec registered as <c>RemoveDuckCommand</c>, which writes the animal id as one variable-length integer and decodes with <see cref="Duck"/> set to <see langword="true"/>.</value>
    public static CommandCodec<RemoveFishingAnimalCommand> DuckCodec { get; } = Create(name: "RemoveDuckCommand", duck: true);

    /// <summary>Gets the contract of the native command that removes a lobster.</summary>
    /// <value>The codec registered as <c>RemoveLobsterCommand</c>, which writes the animal id as one variable-length integer and decodes with <see cref="Duck"/> set to <see langword="false"/>.</value>
    public static CommandCodec<RemoveFishingAnimalCommand> LobsterCodec { get; } = Create(name: "RemoveLobsterCommand", duck: false);

    /// <inheritdoc />
    public override CommandContract Contract => Duck ? DuckCodec : LobsterCodec;

    private static CommandCodec<RemoveFishingAnimalCommand> Create(string name, bool duck)
    {
        return Codecs.Command(name,
            Codecs.Composite(Codecs.VarInt, static command => command.AnimalId, animalId => new RemoveFishingAnimalCommand(animalId, duck)));
    }
}
