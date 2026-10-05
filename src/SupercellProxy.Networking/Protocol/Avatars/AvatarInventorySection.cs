using SupercellProxy.Networking.Protocol.Inventory;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>Named result returned by DecodeInventory.</summary>
internal readonly record struct AvatarInventorySection(int[][] Values, DataReferenceValue[][] Maps, int DeprecatedDataCount, int Unknown0);
