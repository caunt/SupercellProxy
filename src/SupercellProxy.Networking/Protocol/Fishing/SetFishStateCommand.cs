using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Moves one fishing-area fish to the selected runtime state.</summary>
public sealed record SetFishStateCommand([property: System.Text.Json.Serialization.JsonPropertyName("FishGlobalId")] int FishGlobalId, int State) : CodecCommand<SetFishStateCommand>, ICommandCodecProvider<SetFishStateCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetFishStateCommand> Codec { get; } = Codecs.Command(
        type: 112,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.FishGlobalId,
            Codecs.VarInt,
            static command => command.State,
            static (fishGlobalId, state) => new SetFishStateCommand(fishGlobalId, state)
        )
    );
}
