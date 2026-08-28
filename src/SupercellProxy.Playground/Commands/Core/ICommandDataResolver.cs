using System.Diagnostics.CodeAnalysis;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Resolves fields from the live native data tables needed to select polymorphic command payloads.</para>
/// </summary>
internal interface ICommandDataResolver
{
    /// <summary>
    /// Attempts the <c language="csharp">ResolveString</c> operation.
    /// </summary>
    bool TryResolveString(int globalId, string fieldName, [NotNullWhen(true)] out string? value);
}
