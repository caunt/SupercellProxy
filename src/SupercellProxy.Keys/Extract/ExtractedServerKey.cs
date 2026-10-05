namespace SupercellProxy.Keys.Extract;

/// <summary>Key material recovered from an application executable; unknown key versions remain absent.</summary>
internal sealed record ExtractedServerKey(int? KeyVersion, string Key);
