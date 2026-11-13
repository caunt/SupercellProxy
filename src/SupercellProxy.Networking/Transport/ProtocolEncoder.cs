using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Commands;

namespace SupercellProxy.Networking.Transport;

/// <summary>The target every encoder writes protocol values to; <see cref="MessageChannel"/> writes them as wire bytes.</summary>
public abstract class ProtocolEncoder
{
    /// <summary>Gets or sets the runtime environment whose command metadata layout is encoded.</summary>
    /// <value>Build environment that decides which debug and development blocks commands and turns carry; <see cref="CommandEnvironment.Production"/> by default, and a turn message swaps in its own environment while it is written.</value>
    public CommandEnvironment CommandEnvironment { get; set; } = CommandEnvironment.Production;

    /// <summary>Gets or sets the full client release governing versioned layouts.</summary>
    /// <value>Major, minor and patch version of the game client whose message ids and field layouts are used; a <see cref="MessageChannel"/> sets it from the client hello or login it handles. <see langword="null"/> until known, in which case codecs fall back to their default layouts.</value>
    public Version? GameVersion { get; set; }

    /// <summary>Writes raw bytes without a length.</summary>
    /// <param name="source">Bytes written exactly as given; the reader must know how many to expect.</param>
    public abstract void Write(ReadOnlySpan<byte> source);

    /// <summary>Writes a boolean.</summary>
    /// <param name="value">Flag to write; consecutive booleans may be packed into the bits of a single byte.</param>
    public abstract void WriteBoolean(bool value);

    /// <summary>Writes a byte.</summary>
    /// <param name="value">Single unsigned byte to write as is.</param>
    public abstract void WriteByte(byte value);

    /// <summary>Writes a length-prefixed byte array.</summary>
    /// <param name="source">Bytes to write after a 32-bit length; an empty span writes a length of 0 and nothing else.</param>
    public abstract void WriteByteArray(ReadOnlySpan<byte> source);

    /// <summary>Writes a 16-bit integer.</summary>
    /// <param name="value">Signed 16-bit value to write in a fixed two bytes.</param>
    public abstract void WriteInt16(short value);

    /// <summary>Writes a 32-bit integer.</summary>
    /// <param name="value">Signed 32-bit value to write in a fixed four bytes.</param>
    public abstract void WriteInt32(int value);

    /// <summary>Writes a 64-bit integer.</summary>
    /// <param name="value">Signed 64-bit value to write in a fixed eight bytes.</param>
    public abstract void WriteInt64(long value);

    /// <summary>Writes a logic long as its high and low halves.</summary>
    /// <param name="logicLong">Id written as two 32-bit integers, the high word first and the low word second.</param>
    public abstract void WriteLongId(LongId logicLong);

    /// <summary>Writes a byte array that may be absent.</summary>
    /// <param name="source">Bytes to write with a 32-bit length prefix, or <see langword="null"/> (the default) to write a length of -1 that marks the array as absent.</param>
    public abstract void WriteOptionalByteArray(ReadOnlyMemory<byte>? source = null);

    /// <summary>Writes a presence flag followed by the logic long when present.</summary>
    /// <param name="value">Id to write after a <see langword="true"/> flag, or <see langword="null"/> to write only a <see langword="false"/> flag.</param>
    public abstract void WriteOptionalLongId(LongId? value);

    /// <summary>Writes a string that may be absent.</summary>
    /// <param name="value">Text to write as a 32-bit UTF-8 byte length followed by the bytes, or <see langword="null"/> (the default) to write a length of -1.</param>
    public abstract void WriteOptionalString(string? value = null);

    /// <summary>Writes a present string.</summary>
    /// <param name="value">Text to write as a 32-bit UTF-8 byte length followed by the bytes; the empty string writes a length of 0.</param>
    public abstract void WriteString(string value);

    /// <summary>Writes an unsigned 16-bit integer.</summary>
    /// <param name="value">Unsigned 16-bit value to write in a fixed two bytes.</param>
    public abstract void WriteUInt16(ushort value);

    /// <summary>Writes an unsigned 32-bit integer.</summary>
    /// <param name="value">Unsigned 32-bit value to write in a fixed four bytes.</param>
    public abstract void WriteUInt32(uint value);

    /// <summary>Writes an unsigned 64-bit integer.</summary>
    /// <param name="value">Unsigned 64-bit value to write in a fixed eight bytes.</param>
    public abstract void WriteUInt64(ulong value);

    /// <summary>Writes a variable-length 32-bit integer.</summary>
    /// <param name="valueToWrite">Any 32-bit value, negative ones included; small magnitudes take fewer bytes, from one up to five.</param>
    public abstract void WriteVarInt(int valueToWrite);

    /// <summary>Writes a byte array prefixed with a variable-length count.</summary>
    /// <param name="source">Bytes to write after their count encoded as a variable-length integer.</param>
    public abstract void WriteVarIntByteArray(ReadOnlySpan<byte> source);

    /// <summary>Writes a variable-length 64-bit integer.</summary>
    /// <param name="valueToWrite">Any 64-bit value, negative ones included; small magnitudes take fewer bytes, from one up to ten.</param>
    public abstract void WriteVarLong(long valueToWrite);
}
