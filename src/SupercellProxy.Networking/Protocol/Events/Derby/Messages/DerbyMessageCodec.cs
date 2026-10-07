using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

internal static class DerbyMessageCodec
{
    internal static TValue[]? ReadOptionalArray<TValue>(MessageStream stream, Func<MessageStream, TValue> decode)
    {
        int count = stream.ReadVarInt();

        if (count < -1 || count > stream.Length - stream.Position)
            throw new InvalidDataException(message: "Invalid derby collection count.");

        if (count < 0) return null;

        TValue[] result = new TValue[count];

        for (int index = 0; index < result.Length; index++) result[index] = decode(stream);

        return result;
    }

    internal static void RequireEnd(MessageStream stream)
    {
        if (stream.Position != stream.Length)
            throw new InvalidDataException(message: "The derby message has trailing data.");
    }

    internal static void WriteOptionalArray<TValue>(MessageStream stream, TValue[]? entries, Action<MessageStream, TValue> encode)
    {
        stream.WriteVarInt(entries?.Length ?? -1);

        if (entries is null) return;

        foreach (TValue entry in entries) encode(stream, entry);
    }
}
