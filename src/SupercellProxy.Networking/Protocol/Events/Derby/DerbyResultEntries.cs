using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Preserves the optional native result-list owner and its independently nullable list.</summary>
public sealed record DerbyResultEntries(DerbyResultEntry[]? Entries)
{
    /// <summary>Decodes the native bounded nullable result list.</summary>
    public static DerbyResultEntries Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = stream.ReadVarInt();

        if (count == -1)
            return new(Entries: null);

        if (count is < 0 or > 10000)
            throw new InvalidDataException(message: "The derby result list has an invalid count.");

        DerbyResultEntry[] entries = new DerbyResultEntry[count];

        for (int index = 0; index < entries.Length; index++)
            entries[index] = DerbyResultEntry.Decode(stream);

        return new(entries);
    }

    /// <summary>Encodes the nullable result list.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (Entries is null)
        {
            stream.WriteVarInt(valueToWrite: -1);

            return;
        }

        stream.WriteArray<DerbyResultEntry>(Entries, static (writer, value) => value.Encode(writer));
    }
}
