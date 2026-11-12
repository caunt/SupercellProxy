using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// <para>Collects proceeds from one sold roadside listing.</para>
/// </summary>
public sealed record CollectRoadsideSaleProceedsCommand : CodecCommand<CollectRoadsideSaleProceedsCommand>, ICommandCodecProvider<CollectRoadsideSaleProceedsCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CollectRoadsideSaleProceedsCommand"/> record. <para>Initializes collection of a sold listing.</para>
    /// </summary>
    /// <param name="slotIndex">Zero-based position of the sold stand in the own roadside shop, 0 (the first stand) by default.</param>
    public CollectRoadsideSaleProceedsCommand(int slotIndex = 0)
    {
        SlotIndex = slotIndex;
    }

    /// <inheritdoc />
    public static CommandCodec<CollectRoadsideSaleProceedsCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.SlotIndex, static slotIndex => new CollectRoadsideSaleProceedsCommand(slotIndex)),
        CommandLayout.MetadataFirst);

    /// <summary>
    /// Gets the slot index of the sold listing whose coins are collected.
    /// </summary>
    /// <value>The zero-based stand position, written as a var-int after the command metadata.</value>
    public int SlotIndex { get; }
}
