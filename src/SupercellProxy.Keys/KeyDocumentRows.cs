using SupercellProxy.Keys.Models;

namespace SupercellProxy.Keys;

/// <summary>Named result returned by ParseEntries.</summary>
internal readonly record struct KeyDocumentRows(IReadOnlyList<ExistingKeyEntry> Entries, int DataEndIndex);
