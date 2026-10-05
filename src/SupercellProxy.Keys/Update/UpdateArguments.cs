namespace SupercellProxy.Keys.Update;

/// <summary>Named result returned by ParseUpdateArguments.</summary>
internal readonly record struct UpdateArguments(string KeysPath, string? SummaryPath, string? AppOption);
