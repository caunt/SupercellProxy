using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// A town service help request and the home that completed it.
/// </summary>
/// <param name="HelperHomeId">The home that completed the request, or <see langword="null"/> while nobody has helped yet.</param>
/// <param name="HelpId">The request's help id from the avatar's shared help id sequence.</param>
/// <param name="Unknown1">The VarInt written after the help id; its meaning is not yet identified.</param>
/// <param name="Unknown2">The Boolean written last; its meaning is not yet identified.</param>
public sealed record ServiceHelpRequest(LongId? HelperHomeId, int HelpId, int Unknown1, bool Unknown2) : ICodecProvider<ServiceHelpRequest>
{
    /// <inheritdoc />
    public static ICodec<ServiceHelpRequest> Codec { get; } = Codecs.Composite(Codecs.OptionalLongId,
        static value => value.HelperHomeId,
        Codecs.VarInt,
        static value => value.HelpId,
        Codecs.VarInt,
        static value => value.Unknown1,
        Codecs.Boolean,
        static value => value.Unknown2,
        static (helperHomeId, helpId, unknown1, unknown2) => new ServiceHelpRequest(helperHomeId, helpId, unknown1, unknown2));
}
