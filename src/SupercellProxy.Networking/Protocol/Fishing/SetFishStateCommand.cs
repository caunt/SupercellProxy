using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Moves one fishing-area fish to the selected runtime state.</summary>
/// <param name="FishGlobalId">Game object global id of the fish in the fishing area.</param>
/// <param name="State">The native fish state to store, such as 1 for swimming, 2 for baited or 5 for collected, written as a VarInt after the fish id.</param>
public sealed record SetFishStateCommand([property: JsonPropertyName("FishGlobalId")] int FishGlobalId, int State) : CodecCommand<SetFishStateCommand>, ICommandCodecProvider<SetFishStateCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetFishStateCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.FishGlobalId,
        Codecs.VarInt,
        static command => command.State,
        static (fishGlobalId, state) => new SetFishStateCommand(fishGlobalId, state)));
}
