using System.Runtime.InteropServices;

namespace SupercellProxy.Playground.Home;

[StructLayout(LayoutKind.Auto)]
internal readonly record struct BuilderConfiguration(
    int MovementSpeed,
    int IdleMinimumMilliseconds,
    int IdleMaximumMilliseconds
);
