using System.Runtime.InteropServices;

using SupercellProxy.Networking.Protocol.Commands;

namespace SupercellProxy.Networking.Protocol.Versions;

/// <summary>The wire id a release gives a command contract, or none when the release removes it.</summary>
/// <param name="Contract">The command contract.</param>
/// <param name="Id">The wire id from this release onward, or <see langword="null"/> when the contract no longer exists.</param>
[StructLayout(LayoutKind.Auto)]
internal readonly record struct CommandIdAssignment(CommandContract Contract, int? Id);
