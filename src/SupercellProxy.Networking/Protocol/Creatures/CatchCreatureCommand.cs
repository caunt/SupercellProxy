using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Creatures;

/// <summary>Starts catching an existing seasonal creature.</summary>
public sealed record CatchCreatureCommand(int CreatureGlobalId) : CodecCommand<CatchCreatureCommand>, ICommandCodecProvider<CatchCreatureCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CatchCreatureCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.CreatureGlobalId, static creatureGlobalId => new CatchCreatureCommand(creatureGlobalId)),
        CommandLayout.MetadataFirst
    );
}
