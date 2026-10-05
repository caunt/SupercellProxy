using System.Runtime.InteropServices;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandVarIntPair</c>.
/// </summary>
/// <param name="Value0">The first var-length integer.</param>
/// <param name="Value1">The second var-length integer.</param>
[StructLayout(LayoutKind.Auto)]
public readonly record struct CommandVarIntPair(int Value0, int Value1);
