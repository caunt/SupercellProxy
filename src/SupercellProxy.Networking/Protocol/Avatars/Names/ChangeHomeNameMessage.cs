using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Names;

/// <summary>Names the player's farm, as the native tutorial name prompt and the settings name button do.</summary>
/// <param name="Name">The new farm name, written as a length-prefixed string; the server decides whether it accepts it.</param>
public sealed record ChangeHomeNameMessage(string Name) : CodecMessage<ChangeHomeNameMessage>, ICodecProvider<ChangeHomeNameMessage>
{
    /// <inheritdoc />
    public static ICodec<ChangeHomeNameMessage> Codec { get; } = Codecs.Composite(Codecs.String, static message => message.Name, static name => new ChangeHomeNameMessage(name));
}
