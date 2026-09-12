using System.Runtime.InteropServices;

namespace SupercellProxy.Networking.Protocol.ResourceAssociations;

/// Identifies one resource association using its two native unsigned words.
[StructLayout(LayoutKind.Auto)]
public readonly record struct ResourceAssociationKey(uint First, uint Second);
