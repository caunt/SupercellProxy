using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Serverbound command turning a construction site into its finished building; it does nothing before the construction timer
/// ends.
/// </summary>
/// <param name="ConstructionGlobalId">Global id of the construction site's game object.</param>
public sealed record CompleteConstructionCommand([property: JsonPropertyName("ConstructionGlobalId")] int ConstructionGlobalId) : CodecCommand<CompleteConstructionCommand>, ICommandCodecProvider<CompleteConstructionCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CompleteConstructionCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.ConstructionGlobalId, static id => new CompleteConstructionCommand(id))
    );
}
