using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// <para>Collects proceeds from one sold roadside listing.</para>
/// </summary>
public sealed record CollectRoadsideSaleProceedsCommand : CodecCommand<CollectRoadsideSaleProceedsCommand>, ICommandCodecProvider<CollectRoadsideSaleProceedsCommand>
{
    /// <summary>
    /// <para>Initializes collection of a sold listing.</para>
    /// </summary>
    public CollectRoadsideSaleProceedsCommand(int slotIndex = 0)
    {
        SlotIndex = slotIndex;
    }

    /// <inheritdoc />
    public static CommandCodec<CollectRoadsideSaleProceedsCommand> Codec { get; } = Codecs.Command(
        type: 649,
        Codecs.Composite(Codecs.VarInt, static command => command.SlotIndex, static (slotIndex) => new CollectRoadsideSaleProceedsCommand(slotIndex)),
        CommandLayout.MetadataFirst
    );

    /// <summary>
    /// <para>Gets the sold listing slot index.</para>
    /// </summary>
    public int SlotIndex { get; }
}
