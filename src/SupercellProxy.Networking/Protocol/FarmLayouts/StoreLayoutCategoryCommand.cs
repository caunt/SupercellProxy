using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Moves every edit-mode object of one category into storage.</summary>
/// <param name="Category">The object category moved into storage.</param>
public sealed record StoreLayoutCategoryCommand(int Category) : CodecCommand<StoreLayoutCategoryCommand>, ICommandCodecProvider<StoreLayoutCategoryCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StoreLayoutCategoryCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.Category, static category => new StoreLayoutCategoryCommand(category)));
}
