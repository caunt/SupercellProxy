using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Animals;

/// <summary>Starts panic movement for the animals in one habitat.</summary>
public sealed record ScareLivestockCommand(int HabitatGlobalId) : CodecCommand<ScareLivestockCommand>, ICommandCodecProvider<ScareLivestockCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ScareLivestockCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.HabitatGlobalId, static id => new ScareLivestockCommand(id)));
}
