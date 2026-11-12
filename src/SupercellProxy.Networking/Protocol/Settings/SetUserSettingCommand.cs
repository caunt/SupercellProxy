using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings;

/// <summary>Changes a notification or advanced user setting selected by its data row.</summary>
/// <param name="SettingGlobalId">Global id of the setting's data row, sent first as a var-int.</param>
/// <param name="SettingTableId">Number of the game table that row belongs to, which tells a notification setting from an advanced one; sent as a var-int after the global id.</param>
/// <param name="Enabled">The new state of the setting: <see langword="true"/> turns it on, <see langword="false"/> turns it off.</param>
public sealed record SetUserSettingCommand(int SettingGlobalId, int SettingTableId, bool Enabled) : CodecCommand<SetUserSettingCommand>, ICommandCodecProvider<SetUserSettingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetUserSettingCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.SettingGlobalId,
        Codecs.VarInt,
        static command => command.SettingTableId,
        Codecs.Boolean,
        static command => command.Enabled,
        static (settingGlobalId, settingTableId, enabled) => new SetUserSettingCommand(settingGlobalId, settingTableId, enabled)));
}
