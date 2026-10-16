using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Management;

/// <summary>Serverbound request founding a new neighbourhood led by the player; founding costs coins.</summary>
/// <param name="EmblemBackground">Emblem background: a neighbourhood badge global id with the colour in the high byte.</param>
/// <param name="EmblemPattern">Emblem pattern: a neighbourhood badge global id with the colour in the high byte.</param>
/// <param name="EmblemSymbol">Emblem symbol: a neighbourhood badge global id with the colour in the high byte.</param>
/// <param name="Name">Name of the new neighbourhood.</param>
/// <param name="Motto">Short motto shown with the neighbourhood.</param>
/// <param name="Description">Longer text describing the neighbourhood.</param>
/// <param name="Type">Who may join, such as anyone or by request; its values are not identified.</param>
/// <param name="RequiredLevel">Lowest farm level a player needs to join.</param>
/// <param name="FamilyFriendly">Whether the neighbourhood is marked as suitable for all ages.</param>
/// <param name="DerbyEnabled">Whether the neighbourhood takes part in the derby.</param>
/// <param name="Language">Language the neighbourhood uses.</param>
/// <param name="InvitedHomeId">Home of a player invited along at creation.</param>
/// <param name="AutoAccept">Whether join requests are accepted without a member approving them.</param>
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
