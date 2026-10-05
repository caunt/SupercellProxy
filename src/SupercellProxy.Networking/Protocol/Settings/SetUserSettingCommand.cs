using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Settings;

/// <summary>Changes a notification or advanced user setting selected by its data row.</summary>
public sealed record SetUserSettingCommand(int SettingGlobalId, int SettingTableId, bool Enabled) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.SetUserSettingCommandType;

    /// <summary>Decodes the setting reference.</summary>
    public static SetUserSettingCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int settingGlobalId = stream.ReadVarInt();
        int settingTableId = stream.ReadVarInt();
        bool enabled = stream.ReadBoolean();

        return new SetUserSettingCommand(settingGlobalId, settingTableId, enabled);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(SettingGlobalId);
        stream.WriteVarInt(SettingTableId);
        stream.WriteBoolean(Enabled);
    }
}
