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
    public LongId? AccountId { get; init; }

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
    public bool IsSupercellId { get; init; }

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
    public static AccountLoadResponseMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        AccountLoadResponseMessage message = new()
        {
            Value = stream.ReadVarInt(),
            StatusText = stream.ReadOptionalString(),
            AccountId = stream.ReadBoolean() ? stream.ReadLongId() : null,
            AccountToken = stream.ReadOptionalString(),
            IsSupercellId = stream.ReadBoolean(),
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
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteVarInt(Value);
        stream.WriteOptionalString(StatusText);
        stream.WriteBoolean(AccountId is not null);

        if (AccountId is { } accountId)
            stream.WriteLongId(accountId);

        stream.WriteOptionalString(AccountToken);
        stream.WriteBoolean(IsSupercellId);
        Avatar.Encode(stream);
    }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(AccountLoadResponseMessage);
    }
}
