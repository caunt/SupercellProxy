namespace SupercellProxy.Networking.Captures;

/// <summary>Names the packet index that capture writers produce and replay reads.</summary>
public static class CapturePacketIndex
{
    /// <summary>The capture subdirectory that holds the packet index and native traces.</summary>
    public const string DirectoryName = "trace";

    /// <summary>The JSON Lines file with one <see cref="CapturedPacket"/> per saved frame.</summary>
    public const string FileName = "packets.jsonl";
}
