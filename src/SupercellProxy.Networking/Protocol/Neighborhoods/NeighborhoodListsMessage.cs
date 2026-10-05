using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Defines the Neighborhood Lists Message contract.
/// </summary>
public sealed record NeighborhoodListsMessage : IMessage
{
    /// <summary>
    /// Gets the First Entries value.
    /// </summary>
    public NeighborhoodIdValue[]? FirstEntries { get; init; }

    /// <summary>
    /// Gets the Profiles value.
    /// </summary>
    public NeighborhoodProfileValue[]? Profiles { get; init; }

    /// <summary>
    /// Gets the Second Entries value.
    /// </summary>
    public NeighborhoodIdValue[]? SecondEntries { get; init; }

    /// <summary>
    /// Gets the Value value.
    /// </summary>
    public int Value { get; init; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static NeighborhoodListsMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        NeighborhoodIdValue[]? second = ReadIds(stream);
        NeighborhoodIdValue[]? first = ReadIds(stream);
        int count = ReadCount(stream);
        NeighborhoodProfileValue[]? profiles = count < 0 ? null : new NeighborhoodProfileValue[count];

        if (profiles is not null)
        {
            for (int index = 0; index < profiles.Length; index++)
                profiles[index] = new NeighborhoodProfileValue(NeighborhoodProfile.Decode(stream), stream.ReadVarInt());
        }

        NeighborhoodListsMessage message = new()
        {
            FirstEntries = first,
            SecondEntries = second,
            Profiles = profiles,
            Value = stream.ReadVarInt(),
        };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The neighborhood lists contain trailing data.")
            : message;
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        WriteIds(stream, SecondEntries);
        WriteIds(stream, FirstEntries);
        stream.WriteVarInt(Profiles?.Length ?? -1);

        if (Profiles is not null)
        {
            foreach (NeighborhoodProfileValue entry in Profiles)
            {
                entry.Profile.Encode(stream);
                stream.WriteVarInt(entry.Value);
            }
        }

        stream.WriteVarInt(Value);
    }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(NeighborhoodListsMessage);
    }

    private static int ReadCount(MessageStream stream)
    {
        int count = stream.ReadVarInt();

        return count < -1 || count > stream.Length - stream.Position
            ? throw new InvalidDataException(message: "Invalid neighborhood list entry count.")
            : count;
    }

    private static NeighborhoodIdValue[]? ReadIds(MessageStream stream)
    {
        int count = ReadCount(stream);

        if (count < 0)
            return null;

        NeighborhoodIdValue[] entries = new NeighborhoodIdValue[count];

        for (int index = 0; index < entries.Length; index++)
            entries[index] = new NeighborhoodIdValue(stream.ReadLongId(), stream.ReadVarInt());

        return entries;
    }

    private static void WriteIds(MessageStream stream, NeighborhoodIdValue[]? entries)
    {
        stream.WriteVarInt(entries?.Length ?? -1);

        if (entries is not null)
        {
            foreach (NeighborhoodIdValue entry in entries)
            {
                stream.WriteLongId(entry.Id);
                stream.WriteVarInt(entry.Value);
            }
        }
    }
}
