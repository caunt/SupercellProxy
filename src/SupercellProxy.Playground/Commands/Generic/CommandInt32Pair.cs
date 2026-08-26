using System.Runtime.InteropServices;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>CommandInt32Pair</c>.
/// </summary>
/// <param name="Value0">The first fixed-width integer.</param>
/// <param name="Value1">The second fixed-width integer.</param>
[StructLayout(LayoutKind.Auto)]
public readonly record struct CommandInt32Pair(int Value0, int Value1);
