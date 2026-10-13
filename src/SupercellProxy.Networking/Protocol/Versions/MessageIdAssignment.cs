using System.Runtime.InteropServices;

using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.Versions;

/// <summary>The wire id a release gives a message type, or none when the release removes it.</summary>
/// <param name="Entry">The message registration.</param>
/// <param name="Id">The wire id from this release onward, or <see langword="null"/> when the message no longer exists.</param>
[StructLayout(LayoutKind.Auto)]
internal readonly record struct MessageIdAssignment(MessageRegistryEntry Entry, int? Id);
