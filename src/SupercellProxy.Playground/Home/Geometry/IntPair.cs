using System.Runtime.InteropServices;

namespace SupercellProxy.Playground.Home;

[StructLayout(LayoutKind.Auto)]
internal readonly record struct IntPair(int First, int Second);
