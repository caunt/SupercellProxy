using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings;

/// <summary>Changes a notification or advanced user setting selected by its data row.</summary>
public sealed record SetUserSettingCommand(int SettingGlobalId, int SettingTableId, bool Enabled) : CodecCommand<SetUserSettingCommand>, ICommandCodecProvider<SetUserSettingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetUserSettingCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.SettingGlobalId,
            Codecs.VarInt,
            static command => command.SettingTableId,
            Codecs.Boolean,
            static command => command.Enabled,
            static (settingGlobalId, settingTableId, enabled) => new SetUserSettingCommand(settingGlobalId, settingTableId, enabled)
        )
    );
}
