namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

public static partial class CommandRegistry
{
    /// <summary>Resolves a timed derby task with its complete identity.</summary>
    public const int ExpireDerbyTaskServerCommandType = 246;
    /// <summary>Records successful neighborhood help for the active derby task.</summary>
    public const int RecordDerbyHelpServerCommandType = 179;
    /// <summary>Replaces the active derby task's remaining time.</summary>
    public const int SetDerbyTaskTimeServerCommandType = 181;
    /// <summary>Synchronizes the active derby task metadata.</summary>
    public const int SynchronizeDerbyTaskServerCommandType = 234;
}
