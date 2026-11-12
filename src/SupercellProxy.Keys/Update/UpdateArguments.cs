namespace SupercellProxy.Keys.Update;

/// <summary>The command-line options of this command, as <see cref="Application"/> reads them from its arguments.</summary>
internal readonly record struct UpdateArguments(string KeysPath, string? SummaryPath, string? AppOption);
