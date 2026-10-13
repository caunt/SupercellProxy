using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Management;

/// <summary>Changes the settings of the avatar's neighborhood.</summary>
/// <param name="EmblemBackground">The emblem background.</param>
/// <param name="EmblemPattern">The emblem pattern.</param>
/// <param name="EmblemSymbol">The emblem symbol.</param>
/// <param name="Type">The type.</param>
/// <param name="RequiredLevel">The required level.</param>
/// <param name="FamilyFriendly">The family friendly.</param>
/// <param name="DerbyEnabled">The derby enabled.</param>
/// <param name="Motto">The motto.</param>
/// <param name="Description">The description.</param>
/// <param name="Language">The language.</param>
/// <param name="DerbyLeague">The derby league.</param>
/// <param name="Tags">The tags.</param>
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
