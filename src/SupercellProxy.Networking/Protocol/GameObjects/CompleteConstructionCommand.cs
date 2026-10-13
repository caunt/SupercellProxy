using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Defines the Complete Construction Command contract.
/// </summary>
/// <summary>
/// Defines the Construction Global Id contract.
/// </summary>
public sealed record CompleteConstructionCommand([property: System.Text.Json.Serialization.JsonPropertyName("ConstructionGlobalId")] int ConstructionGlobalId) : CodecCommand<CompleteConstructionCommand>, ICommandCodecProvider<CompleteConstructionCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CompleteConstructionCommand> Codec { get; } = Codecs.Command(
        type: 17,
        Codecs.Composite(Codecs.VarInt, static command => command.ConstructionGlobalId, static (id) => new CompleteConstructionCommand(id))
    );
}
