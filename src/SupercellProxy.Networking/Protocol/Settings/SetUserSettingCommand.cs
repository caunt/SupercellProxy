using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Settings;

/// <summary>Changes a notification or advanced user setting selected by its data row.</summary>
public sealed record SetUserSettingCommand(
    int SettingGlobalIdentifier,
    int SettingTableIdentifier,
    bool Enabled,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.SetUserSettingCommandType;

    /// <summary>Decodes the setting reference before the base command fields.</summary>
    public static SetUserSettingCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int settingGlobalIdentifier = stream.ReadVariableInt();
        int settingTableIdentifier = stream.ReadVariableInt();
        bool enabled = stream.ReadBoolean();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new SetUserSettingCommand(settingGlobalIdentifier, settingTableIdentifier, enabled, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(SettingGlobalIdentifier);
        stream.WriteVariableInt(SettingTableIdentifier);
        stream.WriteBoolean(Enabled);
        EncodeCommand(stream, environment);
    }
}
