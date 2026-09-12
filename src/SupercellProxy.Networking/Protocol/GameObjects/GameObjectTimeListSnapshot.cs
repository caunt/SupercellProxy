using SupercellProxy.Networking.Json;


namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Represents decoded <c language="csharp">GameObjectTimeListSnapshot</c> home data.
/// </summary>
public sealed record GameObjectTimeListSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">Action</c> value.
    /// </summary>
    public int Action { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">List</c> value.
    /// </summary>
    public EncodedDocumentValue[] List { get; init; } = [];
}
