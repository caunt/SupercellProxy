namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// Represents <c language="csharp">GameDataTable</c>.
/// </summary>
public sealed record GameDataTable(IReadOnlyList<string> Headers, IReadOnlyList<string> Types, IReadOnlyList<GameDataTableEntry> Entries);
