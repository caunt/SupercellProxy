using System.Globalization;

using SupercellProxy.Networking.Protocol.Avatars;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Accounts;

/// <summary>
/// Defines the Account Load Response Message contract.
/// </summary>
public sealed record AccountLoadResponseMessage : IMessage
{

    /// <summary>
    /// Gets the Account Id value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("AccountId")]
    public LongIdentifier? AccountIdentifier { get; init; }

    /// <summary>
    /// Gets the Account Token value.
    /// </summary>
    public string? AccountToken { get; init; }

    /// <summary>
    /// Gets the Avatar value.
    /// </summary>
    public ClientAvatar Avatar { get; init; } = new();

    /// <summary>
    /// Gets the Is Supercell Id value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("IsSupercellId")]
    public bool IsSupercellIdentifier { get; init; }

    /// <summary>
    /// Gets the Status Text value.
    /// </summary>
    public string? StatusText { get; init; }
    /// <summary>
    /// Gets the Value value.
    /// </summary>
    public int Value { get; init; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AccountLoadResponseMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;

        AccountLoadResponseMessage message = new()
        {
            Value = stream.ReadVariableInt(),
            StatusText = stream.ReadOptionalString(),
            AccountIdentifier = stream.ReadBoolean() ? stream.ReadLongIdentifier() : null,
            AccountToken = stream.ReadOptionalString(),
            IsSupercellIdentifier = stream.ReadBoolean(),
            Avatar = ClientAvatar.Decode(stream),
        };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(
                string.Create(CultureInfo.InvariantCulture, $"Unexpected trailing account-load response data at {stream.Position} of {stream.Length}.")
            )
            : message;
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(Value);
        stream.WriteOptionalString(StatusText);
        stream.WriteBoolean(AccountIdentifier is not null);

        if (AccountIdentifier is { } accountIdentifier)
            stream.WriteLongIdentifier(accountIdentifier);

        stream.WriteOptionalString(AccountToken);
        stream.WriteBoolean(IsSupercellIdentifier);
        Avatar.Encode(stream);

        return new MessageContainer(identifier, version, stream);
    }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(AccountLoadResponseMessage);
    }
}
