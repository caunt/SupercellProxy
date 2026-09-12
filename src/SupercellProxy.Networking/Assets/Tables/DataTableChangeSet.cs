using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>Represents the decoded DataTableChangeSet JSON contract.</summary>
public sealed record DataTableChangeSet
{
    /// <summary>Gets the Changes value.</summary>
    [JsonPropertyName("Changes")]
    public DataTableChange[]? Changes { get; init; }

}
