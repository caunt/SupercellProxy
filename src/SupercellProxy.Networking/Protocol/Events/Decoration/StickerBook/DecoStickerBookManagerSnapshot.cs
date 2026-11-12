using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.StickerBook;

/// <summary>The saved decoration sticker book: whether the player has opened it and the progress of each collection.</summary>
public sealed record DecoStickerBookManagerSnapshot
{
    /// <summary>Gets or sets the saved progress of each collection, in sticker-book collection table order.</summary>
    /// <value>Entry <c>n</c> belongs to row <c>n</c> of the sticker-book collections table, and <c>n</c> is the collection index the claim commands use; empty before any collection was saved.</value>
    [JsonPropertyName("collections")]
    public DecoStickerBookCollectionSnapshot[] Collections { get; init; } = [];

    /// <summary>Gets or sets whether the player has opened the sticker book.</summary>
    /// <value><see langword="true"/> if the sticker book was marked opened (saved as <c>playerOpened</c>); otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("playerOpened")]
    public bool PlayerOpened { get; init; }
}
