using System.Runtime.InteropServices;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

[StructLayout(LayoutKind.Auto)]
internal readonly record struct IntPair(int First, int Second);
