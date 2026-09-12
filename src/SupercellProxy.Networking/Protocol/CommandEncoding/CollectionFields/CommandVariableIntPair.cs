using System.Runtime.InteropServices;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandVarIntPair</c>.
/// </summary>
/// <param name="Value0">The first variable-length integer.</param>
/// <param name="Value1">The second variable-length integer.</param>
[StructLayout(LayoutKind.Auto)]
public readonly record struct CommandVariableIntPair(int Value0, int Value1);
