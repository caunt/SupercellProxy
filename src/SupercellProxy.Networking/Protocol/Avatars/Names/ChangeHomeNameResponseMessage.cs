using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Names;

/// <summary>Answers a farm name request; native keeps the name only when <see cref="Result"/> is <see cref="Accepted"/>.</summary>
/// <param name="Result">The native result: 0 failed, 1 accepted, 2 rejected as a bad word.</param>
/// <param name="Name">The name the server answered for.</param>
public sealed record ChangeHomeNameResponseMessage(int Result, string? Name) : CodecMessage<ChangeHomeNameResponseMessage>, ICodecProvider<ChangeHomeNameResponseMessage>
{
    /// <summary>The result native shows as a kept name.</summary>
    public const int Accepted = 1;

    /// <inheritdoc />
    public static ICodec<ChangeHomeNameResponseMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.Result,
        Codecs.OptionalString,
        static message => message.Name,
        static (result, name) => new ChangeHomeNameResponseMessage(result, name)
    );
}
