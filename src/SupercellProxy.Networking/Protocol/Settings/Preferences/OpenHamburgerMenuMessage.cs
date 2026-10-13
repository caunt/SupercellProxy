using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings.Preferences;

/// <summary>Reports a hamburger menu action.</summary>
/// <param name="Action">The action.</param>
public sealed record OpenHamburgerMenuMessage(int Action) : CodecMessage<OpenHamburgerMenuMessage>, ICodecProvider<OpenHamburgerMenuMessage>
{
    /// <inheritdoc />
    public static ICodec<OpenHamburgerMenuMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.Action, static (action) => new OpenHamburgerMenuMessage(action));
}
