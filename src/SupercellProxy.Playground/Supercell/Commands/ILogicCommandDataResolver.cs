using System.Diagnostics.CodeAnalysis;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Resolves fields from the live native data tables needed to select polymorphic command payloads.
/// </summary>
public interface ILogicCommandDataResolver
{
    bool TryResolveString(int globalId, string fieldName, [NotNullWhen(true)] out string? value);
}
