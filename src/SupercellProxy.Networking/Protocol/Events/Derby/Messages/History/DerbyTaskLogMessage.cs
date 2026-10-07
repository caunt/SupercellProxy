using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.History;

/// <summary>Returns both native member collections for the neighborhood's derby task log.</summary>
public sealed record DerbyTaskLogMessage : IMessage
{
    /// <summary>Gets the first optional collection in native order.</summary>
    public DerbyMemberTaskLogEntry[]? FirstEntries { get; init; }
    /// <summary>Gets the second optional collection in native order.</summary>
    public DerbyMemberTaskLogEntry[]? SecondEntries { get; init; }
    /// <summary>Gets the first value following the two collections.</summary>
    public int Value0 { get; init; }
    /// <summary>Gets the second value following the two collections.</summary>
    public int Value1 { get; init; }

    /// <summary>Decodes the complete task-log response, preserving absent collections.</summary>
    public static DerbyTaskLogMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        DerbyTaskLogMessage message = new()
        {
            FirstEntries = DerbyMessageCodec.ReadOptionalArray(stream, DerbyMemberTaskLogEntry.Decode),
            SecondEntries = DerbyMessageCodec.ReadOptionalArray(stream, DerbyMemberTaskLogEntry.Decode),
            Value0 = stream.ReadVarInt(),
            Value1 = stream.ReadVarInt(),
        };

        DerbyMessageCodec.RequireEnd(stream);

        return message;
    }

    /// <summary>Encodes the collections followed by the two native values.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        DerbyMessageCodec.WriteOptionalArray(stream, FirstEntries, static (writer, entry) => entry.Encode(writer));
        DerbyMessageCodec.WriteOptionalArray(stream, SecondEntries, static (writer, entry) => entry.Encode(writer));
        stream.WriteVarInt(Value0);
        stream.WriteVarInt(Value1);
    }

    /// <summary>Omits member details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DerbyTaskLogMessage);
    }
}
