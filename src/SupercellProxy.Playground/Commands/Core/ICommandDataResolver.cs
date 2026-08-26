using System.Diagnostics.CodeAnalysis;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Resolves fields from the live native data tables needed to select polymorphic command payloads.</para>
/// </summary>
public interface ICommandDataResolver
{
    /// <summary>
    /// Attempts the <c>ResolveString</c> operation.
    /// </summary>
    bool TryResolveString(int globalId, string fieldName, [NotNullWhen(true)] out string? value);
}
