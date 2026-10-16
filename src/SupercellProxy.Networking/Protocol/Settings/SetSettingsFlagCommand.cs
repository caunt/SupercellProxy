using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings;

/// <summary>Sets the boolean flag held by the native client settings singleton.</summary>
/// <param name="Enabled">Whether the setting is enabled.</param>
public sealed record SetSettingsFlagCommand(bool Enabled) : CodecCommand<SetSettingsFlagCommand>, ICommandCodecProvider<SetSettingsFlagCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetSettingsFlagCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.Boolean, static command => command.Enabled, static enabled => new SetSettingsFlagCommand(enabled)));
}
