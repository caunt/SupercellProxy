using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Members;

/// <summary>Delivers entries that each carry a member list to the neighborhood members manager.</summary>
/// <param name="Entries">The member lists, each with its owner and members.</param>
public sealed record NeighborhoodMemberListsMessage(ReadOnlyMemory<NeighborhoodMemberListEntry> Entries) : CodecMessage<NeighborhoodMemberListsMessage>, ICodecProvider<NeighborhoodMemberListsMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodMemberListsMessage> Codec { get; } = Codecs.Composite(
        Codecs.Memory(NeighborhoodMemberListEntry.Codec),
        static message => message.Entries,
        static entries => new NeighborhoodMemberListsMessage(entries)
    );
}
