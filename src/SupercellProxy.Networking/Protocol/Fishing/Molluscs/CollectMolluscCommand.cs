using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing.Molluscs;

/// <summary>Collects one grown mollusc from its existing bed.</summary>
/// <param name="MolluscId">Game object global id of the grown mollusc on its bed, not the id of its data row, written as a VarInt.</param>
public sealed record CollectMolluscCommand(int MolluscId) : CodecCommand<CollectMolluscCommand>, ICommandCodecProvider<CollectMolluscCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectMolluscCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.MolluscId, static molluscId => new CollectMolluscCommand(molluscId)));
}
