using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.StickerBook;

/// <summary>The saved decoration sticker book: whether the player has opened it and the progress of each collection.</summary>
public sealed record DecoStickerBookManagerSnapshot
{
    /// <summary>Gets the saved progress of each collection, in sticker-book collection table order.</summary>
    [JsonPropertyName("collections")]
    public DecoStickerBookCollectionSnapshot[] Collections { get; init; } = [];

    /// <summary>Gets whether the player has opened the sticker book.</summary>
    [JsonPropertyName("playerOpened")]
    public bool PlayerOpened { get; init; }
}
