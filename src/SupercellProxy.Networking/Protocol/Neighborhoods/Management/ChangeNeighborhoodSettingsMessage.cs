using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Management;

/// <summary>Changes the settings of the avatar's neighborhood.</summary>
/// <param name="EmblemBackground">Emblem background: a neighbourhood badge global id with the colour in the high byte.</param>
/// <param name="EmblemPattern">Emblem pattern: a neighbourhood badge global id with the colour in the high byte.</param>
/// <param name="EmblemSymbol">Emblem symbol: a neighbourhood badge global id with the colour in the high byte.</param>
/// <param name="Type">Who may join, such as anyone or by request; its values are not identified.</param>
/// <param name="RequiredLevel">Lowest farm level a player needs to join.</param>
/// <param name="FamilyFriendly">Whether the neighbourhood is marked as suitable for all ages.</param>
/// <param name="DerbyEnabled">Whether the neighbourhood takes part in the derby.</param>
/// <param name="Motto">Short motto shown with the neighbourhood.</param>
/// <param name="Description">Longer text describing the neighbourhood.</param>
/// <param name="Language">Language the neighbourhood uses.</param>
/// <param name="DerbyLeague">Derby league the neighbourhood plays in; its values are not identified.</param>
/// <param name="Tags">Tags describing the neighbourhood's play style, packed into one number; the packing is not identified.</param>
public sealed record ChangeNeighborhoodSettingsMessage(
    int EmblemBackground,
    int EmblemPattern,
    int EmblemSymbol,
    int Type,
    int RequiredLevel,
    bool FamilyFriendly,
    bool DerbyEnabled,
    string? Motto,
    string? Description,
    string? Language,
    int DerbyLeague,
    int Tags
) : CodecMessage<ChangeNeighborhoodSettingsMessage>, ICodecProvider<ChangeNeighborhoodSettingsMessage>
{
    /// <inheritdoc />
    public static ICodec<ChangeNeighborhoodSettingsMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.EmblemBackground,
        Codecs.VarInt,
        static message => message.EmblemPattern,
        Codecs.VarInt,
        static message => message.EmblemSymbol,
        Codecs.VarInt,
        static message => message.Type,
        Codecs.VarInt,
        static message => message.RequiredLevel,
        Codecs.Boolean,
        static message => message.FamilyFriendly,
        Codecs.Boolean,
        static message => message.DerbyEnabled,
        Codecs.OptionalString,
        static message => message.Motto,
        Codecs.OptionalString,
        static message => message.Description,
        Codecs.OptionalString,
        static message => message.Language,
        Codecs.Int32,
        static message => message.DerbyLeague,
        Codecs.Int32,
        static message => message.Tags,
        static (
            emblemBackground,
            emblemPattern,
            emblemSymbol,
            type,
            requiredLevel,
            familyFriendly,
            derbyEnabled,
            motto,
            description,
            language,
            derbyLeague,
            tags
        ) => new ChangeNeighborhoodSettingsMessage(
            emblemBackground,
            emblemPattern,
            emblemSymbol,
            type,
            requiredLevel,
            familyFriendly,
            derbyEnabled,
            motto,
            description,
            language,
            derbyLeague,
            tags
        )
    );
}
