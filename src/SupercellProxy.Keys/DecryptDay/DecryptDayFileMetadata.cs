using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>One decrypted app file decrypt.day offers, with whether it can be downloaded without an account.</summary>
internal sealed record DecryptDayFileMetadata
{
    /// <summary>decrypt.day's identifier of the file, used to download it.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>Whether downloading the file needs a decrypt.day login; such files are skipped.</summary>
    [JsonPropertyName("login_required")]
    public bool? LoginRequired { get; init; }

    /// <summary>Whether the file is for paying decrypt.day members only; such files are skipped.</summary>
    [JsonPropertyName("premium")]
    public bool? Premium { get; init; }
}
