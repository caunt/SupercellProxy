using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Gatherers;

/// <summary>Collects the completed product from a gatherer nest.</summary>
public sealed record CollectGathererNestCommand([property: System.Text.Json.Serialization.JsonPropertyName("GathererNestGlobalId")] int GathererNestGlobalId) : CodecCommand<CollectGathererNestCommand>, ICommandCodecProvider<CollectGathererNestCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectGathererNestCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.GathererNestGlobalId, static (id) => new CollectGathererNestCommand(id))
    );
}
