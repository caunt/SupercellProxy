using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Accounts;

/// <summary>
/// Defines the Clientbound24843 Message contract.
/// </summary>
public sealed record AccountCandidatesMessage : IMessage
{
    /// <summary>
    /// Gets the Entries value.
    /// </summary>
    public AccountCandidateEntry[]? Entries { get; init; } = [];

    /// <summary>
    /// Gets the Entry Count value.
    /// </summary>
    public int EntryCount => Entries?.Length ?? -1;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AccountCandidatesMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        int count = container.Payload.ReadVariableInt();

        if (count < -1 || count > (container.Payload.Length - container.Payload.Position) / 28)
            throw new InvalidDataException(message: "The account-entry response has an invalid payload.");

        AccountCandidateEntry[]? entries = count < 0 ? null : new AccountCandidateEntry[count];

        if (entries is not null)
        {
            for (int index = 0; index < entries.Length; index++)
                entries[index] = AccountCandidateEntry.Decode(container.Payload);
        }

        return container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "The account-entry response has trailing data.")
            : new AccountCandidatesMessage { Entries = entries };
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(EntryCount);

        if (Entries is not null)
        {
            foreach (AccountCandidateEntry entry in Entries)
                entry.Encode(stream);
        }

        return new MessageContainer(identifier, version, stream);
    }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(AccountCandidatesMessage);
    }
}
