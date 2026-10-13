using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// A town service help request and the home that completed it.
/// </summary>
public sealed record ServiceHelpRequest(LongId? HelperHomeId, int HelpId, int Unknown1, bool Unknown2) : ICodecProvider<ServiceHelpRequest>
{
    /// <inheritdoc />
    public static ICodec<ServiceHelpRequest> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static value => value.HelperHomeId,
        Codecs.VarInt,
        static value => value.HelpId,
        Codecs.VarInt,
        static value => value.Unknown1,
        Codecs.Boolean,
        static value => value.Unknown2,
        static (helperHomeId, helpId, unknown1, unknown2) => new ServiceHelpRequest(helperHomeId, helpId, unknown1, unknown2)
    );
}
