using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>Configures the native platform profiler and its segment reporting.</summary>
public sealed record PerformanceProfilingSettingsMessage(bool UseDefaultJsonCallback, int Mode, bool Enabled) : IMessage
{
    internal static Version ReorderedFieldsVersion { get; } = new(major: 1, minor: 73, build: 81);

    /// <summary>Reads the profiler callback flag, mode, and enable flag in native wire order.</summary>
    public static PerformanceProfilingSettingsMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (stream.GameVersion >= ReorderedFieldsVersion)
            return new(stream.ReadBoolean(), stream.ReadVarInt(), stream.ReadBoolean());

        int mode = stream.ReadVarInt();
        bool enabled = stream.ReadBoolean();

        return new(stream.ReadBoolean(), mode, enabled);
    }

    /// <summary>Writes the platform profiling settings without changing farm simulation state.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (stream.GameVersion >= ReorderedFieldsVersion)
        {
            stream.WriteBoolean(UseDefaultJsonCallback);
            stream.WriteVarInt(Mode);
            stream.WriteBoolean(Enabled);
        }
        else
        {
            stream.WriteVarInt(Mode);
            stream.WriteBoolean(Enabled);
            stream.WriteBoolean(UseDefaultJsonCallback);
        }
    }
}
