using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Streams;

/// <summary>One inbox (avatar stream) entry; native picks the entry class from the type that precedes it.</summary>
/// <param name="Header">The fields every entry shares.</param>
public abstract record AvatarStreamEntry(AvatarStreamEntryHeader Header) : ICodecProvider<AvatarStreamEntry>
{
    /// <summary>
    /// Gets the wire codec: the entry type selects the entry record.
    /// </summary>
    public static ICodec<AvatarStreamEntry> Codec { get; } = Codecs.Dispatch(
        Codecs.VarInt,
        static entry => entry.EntryType,
        [
            new KeyValuePair<int, ICodec<AvatarStreamEntry>>(
                JoinNeighborhoodResponseAvatarStreamEntry.Type,
                Codecs.Subtype<AvatarStreamEntry, JoinNeighborhoodResponseAvatarStreamEntry>(JoinNeighborhoodResponseAvatarStreamEntry.Codec)
            ),
            new KeyValuePair<int, ICodec<AvatarStreamEntry>>(
                NeighborhoodInvitationAvatarStreamEntry.Type,
                Codecs.Subtype<AvatarStreamEntry, NeighborhoodInvitationAvatarStreamEntry>(NeighborhoodInvitationAvatarStreamEntry.Codec)
            ),
            new KeyValuePair<int, ICodec<AvatarStreamEntry>>(
                NeighborhoodKickOutStreamEntry.Type,
                Codecs.Subtype<AvatarStreamEntry, NeighborhoodKickOutStreamEntry>(NeighborhoodKickOutStreamEntry.Codec)
            ),
            new KeyValuePair<int, ICodec<AvatarStreamEntry>>(
                NeighborhoodSettingsChangedStreamEntry.Type,
                Codecs.Subtype<AvatarStreamEntry, NeighborhoodSettingsChangedStreamEntry>(NeighborhoodSettingsChangedStreamEntry.Codec)
            ),
            new KeyValuePair<int, ICodec<AvatarStreamEntry>>(
                NeighborhoodDerbyOptStreamEntry.Type,
                Codecs.Subtype<AvatarStreamEntry, NeighborhoodDerbyOptStreamEntry>(NeighborhoodDerbyOptStreamEntry.Codec)
            ),
        ]
    );

    /// <summary>Gets the native entry type written before the entry.</summary>
    public abstract int EntryType { get; }
}
