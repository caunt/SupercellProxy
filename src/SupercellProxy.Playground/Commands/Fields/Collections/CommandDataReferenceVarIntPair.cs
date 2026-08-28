using System.Runtime.InteropServices;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">CommandDataReferenceVarIntPair</c>.
/// </summary>
/// <param name="GlobalId">The referenced data global ID.</param>
/// <param name="Value">The associated variable-length integer.</param>
[StructLayout(LayoutKind.Auto)]
internal readonly record struct CommandDataReferenceVarIntPair(int GlobalId, int Value);
