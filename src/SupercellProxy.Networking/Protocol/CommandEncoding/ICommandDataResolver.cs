using System.Diagnostics.CodeAnalysis;

namespace SupercellProxy.Networking.Protocol.CommandEncoding;

/// <summary>
/// <para>Resolves fields from the live native data tables needed to select polymorphic command payloads.</para>
/// </summary>
public interface ICommandDataResolver
{
    /// <summary>
    /// Attempts the <c language="csharp">ResolveString</c> operation.
    /// </summary>
    bool TryResolveString(int globalIdentifier, string fieldName, [NotNullWhen(true)] out string? value);
}
