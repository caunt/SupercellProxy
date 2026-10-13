using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Pets;

/// <summary>Fills one available pet bowl and spends the supplied food quantity.</summary>
public sealed record FeedPetHabitatCommand(int HabitatGlobalId, int FoodGlobalId, int Quantity) : CodecCommand<FeedPetHabitatCommand>, ICommandCodecProvider<FeedPetHabitatCommand>
{
    /// <inheritdoc />
    public static CommandCodec<FeedPetHabitatCommand> Codec { get; } = Codecs.Command(
        type: 86,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.HabitatGlobalId,
            Codecs.VarInt,
            static command => command.FoodGlobalId,
            Codecs.VarInt,
            static command => command.Quantity,
            static (habitatGlobalId, foodGlobalId, quantity) => new FeedPetHabitatCommand(habitatGlobalId, foodGlobalId, quantity)
        )
    );
}
