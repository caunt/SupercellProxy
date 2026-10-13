using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Commands;

namespace SupercellProxy.Networking.Transport;

/// <summary>The target every encoder writes protocol values to; <see cref="MessageStream"/> writes them as wire bytes.</summary>
public abstract class ProtocolEncoder
{
    /// <summary>Gets or sets the runtime environment whose command metadata layout is encoded.</summary>
    public CommandEnvironment CommandEnvironment { get; set; } = CommandEnvironment.Production;

    /// <summary>Gets or sets the full client release governing versioned layouts.</summary>
    public Version? GameVersion { get; set; }

    /// <summary>Writes raw bytes without a length.</summary>
    public abstract void Write(ReadOnlySpan<byte> source);

    /// <summary>Writes a boolean.</summary>
    public abstract void WriteBoolean(bool value);

    /// <summary>Writes a byte.</summary>
    public abstract void WriteByte(byte value);

    /// <summary>Writes a length-prefixed byte array.</summary>
    public abstract void WriteByteArray(ReadOnlySpan<byte> source);

    /// <summary>Writes a 16-bit integer.</summary>
    public abstract void WriteInt16(short value);

    /// <summary>Writes a 32-bit integer.</summary>
    public abstract void WriteInt32(int value);

    /// <summary>Writes a 64-bit integer.</summary>
    public abstract void WriteInt64(long value);

    /// <summary>Writes a logic long as its high and low halves.</summary>
    public abstract void WriteLongId(LongId logicLong);

    /// <summary>Writes a byte array that may be absent.</summary>
    public abstract void WriteOptionalByteArray(ReadOnlyMemory<byte>? source = null);

    /// <summary>Writes a presence flag followed by the logic long when present.</summary>
    public abstract void WriteOptionalLongId(LongId? value);

    /// <summary>Writes a string that may be absent.</summary>
    public abstract void WriteOptionalString(string? value = null);

    /// <summary>Writes a present string.</summary>
    public abstract void WriteString(string value);

    /// <summary>Writes an unsigned 16-bit integer.</summary>
    public abstract void WriteUInt16(ushort value);

    /// <summary>Writes an unsigned 32-bit integer.</summary>
    public abstract void WriteUInt32(uint value);

    /// <summary>Writes an unsigned 64-bit integer.</summary>
    public abstract void WriteUInt64(ulong value);

    /// <summary>Writes a variable-length 32-bit integer.</summary>
    public abstract void WriteVarInt(int valueToWrite);

    /// <summary>Writes a byte array prefixed with a variable-length count.</summary>
    public abstract void WriteVarIntByteArray(ReadOnlySpan<byte> source);

    /// <summary>Writes a variable-length 64-bit integer.</summary>
    public abstract void WriteVarLong(long valueToWrite);
}
