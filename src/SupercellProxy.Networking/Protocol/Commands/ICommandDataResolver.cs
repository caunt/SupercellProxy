using System.Diagnostics.CodeAnalysis;

namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>
/// <para>Resolves fields from the live native data tables needed to select polymorphic command payloads.</para>
/// </summary>
public interface ICommandDataResolver
{
    /// <summary>
    /// Attempts the <c language="csharp">ResolveString</c> operation.
    /// </summary>
    /// <param name="globalId">The global id of the data table entry to read, typically taken from an earlier field of the payload being decoded.</param>
    /// <param name="fieldName">The column header to read, such as <c>TaskType</c>.</param>
    /// <param name="value">When this method returns <see langword="true"/>, the cell's text; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="globalId"/> names a known entry that has a <paramref name="fieldName"/> column and that cell is not empty; otherwise, <see langword="false"/>.</returns>
    bool TryResolveString(int globalId, string fieldName, [NotNullWhen(true)] out string? value);
}
