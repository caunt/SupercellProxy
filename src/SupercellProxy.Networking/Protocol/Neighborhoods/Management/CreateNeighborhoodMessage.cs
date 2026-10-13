using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Management;

/// <summary>Creates a neighborhood.</summary>
/// <param name="EmblemBackground">The emblem background.</param>
/// <param name="EmblemPattern">The emblem pattern.</param>
/// <param name="EmblemSymbol">The emblem symbol.</param>
/// <param name="Name">The name.</param>
/// <param name="Motto">The motto.</param>
/// <param name="Description">The description.</param>
/// <param name="Type">The type.</param>
/// <param name="RequiredLevel">The required level.</param>
/// <param name="FamilyFriendly">The family friendly.</param>
/// <param name="DerbyEnabled">The derby enabled.</param>
/// <param name="Language">The language.</param>
/// <param name="InvitedHomeId">The invited home id.</param>
/// <param name="AutoAccept">The auto accept.</param>
public sealed record CreateNeighborhoodMessage(
    int EmblemBackground,
    int EmblemPattern,
    int EmblemSymbol,
    string? Name,
    string? Motto,
    string? Description,
    int Type,
    int RequiredLevel,
    bool FamilyFriendly,
    bool DerbyEnabled,
    string? Language,
    LongId InvitedHomeId,
    bool AutoAccept
) : CodecMessage<CreateNeighborhoodMessage>, ICodecProvider<CreateNeighborhoodMessage>
{
    /// <inheritdoc />
    public static ICodec<CreateNeighborhoodMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.EmblemBackground,
        Codecs.VarInt,
        static message => message.EmblemPattern,
        Codecs.VarInt,
        static message => message.EmblemSymbol,
        Codecs.OptionalString,
        static message => message.Name,
        Codecs.OptionalString,
        static message => message.Motto,
        Codecs.OptionalString,
        static message => message.Description,
        Codecs.VarInt,
        static message => message.Type,
        Codecs.VarInt,
        static message => message.RequiredLevel,
        Codecs.Boolean,
        static message => message.FamilyFriendly,
        Codecs.Boolean,
        static message => message.DerbyEnabled,
        Codecs.OptionalString,
        static message => message.Language,
        Codecs.LongId,
        static message => message.InvitedHomeId,
        Codecs.Boolean,
        static message => message.AutoAccept,
        static (
            emblemBackground,
            emblemPattern,
            emblemSymbol,
            name,
            motto,
            description,
            type,
            requiredLevel,
            familyFriendly,
            derbyEnabled,
            language,
            invitedHomeId,
            autoAccept
        ) => new CreateNeighborhoodMessage(
            emblemBackground,
            emblemPattern,
            emblemSymbol,
            name,
            motto,
            description,
            type,
            requiredLevel,
            familyFriendly,
            derbyEnabled,
            language,
            invitedHomeId,
            autoAccept
        )
    );
}
