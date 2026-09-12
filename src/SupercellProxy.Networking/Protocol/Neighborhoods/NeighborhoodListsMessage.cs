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
    public NeighborhoodIdentifierValue[]? FirstEntries { get; init; }

    /// <summary>
    /// Gets the Profiles value.
    /// </summary>
    public NeighborhoodProfileValue[]? Profiles { get; init; }

    /// <summary>
    /// Gets the Second Entries value.
    /// </summary>
    public NeighborhoodIdentifierValue[]? SecondEntries { get; init; }

    /// <summary>
    /// Gets the Value value.
    /// </summary>
    public int Value { get; init; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static NeighborhoodListsMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;
        NeighborhoodIdentifierValue[]? second = ReadIdentifiers(stream);
        NeighborhoodIdentifierValue[]? first = ReadIdentifiers(stream);
        int count = ReadCount(stream);
        NeighborhoodProfileValue[]? profiles = count < 0 ? null : new NeighborhoodProfileValue[count];

        if (profiles is not null)
        {
            for (int index = 0; index < profiles.Length; index++)
                profiles[index] = new NeighborhoodProfileValue(NeighborhoodProfile.Decode(stream), stream.ReadVariableInt());
        }

        NeighborhoodListsMessage message = new()
        {
            FirstEntries = first,
            SecondEntries = second,
            Profiles = profiles,
            Value = stream.ReadVariableInt(),
        };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The neighborhood lists contain trailing data.")
            : message;
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        WriteIdentifiers(stream, SecondEntries);
        WriteIdentifiers(stream, FirstEntries);
        stream.WriteVariableInt(Profiles?.Length ?? -1);

        if (Profiles is not null)
        {
            foreach (NeighborhoodProfileValue entry in Profiles)
            {
                entry.Profile.Encode(stream);
                stream.WriteVariableInt(entry.Value);
            }
        }

        stream.WriteVariableInt(Value);

        return new MessageContainer(identifier, version, stream);
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
        int count = stream.ReadVariableInt();

        return count < -1 || count > stream.Length - stream.Position
            ? throw new InvalidDataException(message: "Invalid neighborhood list entry count.")
            : count;
    }

    private static NeighborhoodIdentifierValue[]? ReadIdentifiers(MessageStream stream)
    {
        int count = ReadCount(stream);

        if (count < 0)
            return null;

        NeighborhoodIdentifierValue[] entries = new NeighborhoodIdentifierValue[count];

        for (int index = 0; index < entries.Length; index++)
            entries[index] = new NeighborhoodIdentifierValue(stream.ReadLongIdentifier(), stream.ReadVariableInt());

        return entries;
    }

    private static void WriteIdentifiers(MessageStream stream, NeighborhoodIdentifierValue[]? entries)
    {
        stream.WriteVariableInt(entries?.Length ?? -1);

        if (entries is not null)
        {
            foreach (NeighborhoodIdentifierValue entry in entries)
            {
                stream.WriteLongIdentifier(entry.Identifier);
                stream.WriteVariableInt(entry.Value);
            }
        }
    }
}
