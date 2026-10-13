using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Helpers.Boy;

/// <summary>Sends the boy to search for an item.</summary>
/// <param name="ItemGlobalId">The searched item data id.</param>
public sealed record SearchWithBoyCommand(int ItemGlobalId) : CodecCommand<SearchWithBoyCommand>, ICommandCodecProvider<SearchWithBoyCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SearchWithBoyCommand> Codec { get; } = Codecs.Command(
        type: 71,
        Codecs.Composite(Codecs.VarInt, static command => command.ItemGlobalId, static (itemGlobalId) => new SearchWithBoyCommand(itemGlobalId))
    );
}
