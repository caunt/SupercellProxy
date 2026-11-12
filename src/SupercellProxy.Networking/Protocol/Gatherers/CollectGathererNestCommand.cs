using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Gatherers;

/// <summary>Collects the completed product from a gatherer nest.</summary>
/// <param name="GathererNestGlobalId">Global id of the placed nest object to collect from, written as a VarInt.</param>
public sealed record CollectGathererNestCommand([property: JsonPropertyName("GathererNestGlobalId")] int GathererNestGlobalId) : CodecCommand<CollectGathererNestCommand>, ICommandCodecProvider<CollectGathererNestCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectGathererNestCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.GathererNestGlobalId, static id => new CollectGathererNestCommand(id)));
}
