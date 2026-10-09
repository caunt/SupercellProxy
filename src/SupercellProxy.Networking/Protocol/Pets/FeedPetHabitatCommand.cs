using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Pets;

/// <summary>Fills one available pet bowl and spends the supplied food quantity.</summary>
public sealed record FeedPetHabitatCommand(int HabitatGlobalId, int FoodGlobalId, int Quantity) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.FeedPetHabitatCommandType;

    /// <summary>Decodes the habitat, food reference and quantity before the command base.</summary>
    public static FeedPetHabitatCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new FeedPetHabitatCommand(stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(HabitatGlobalId);
        stream.WriteVarInt(FoodGlobalId);
        stream.WriteVarInt(Quantity);
    }
}
