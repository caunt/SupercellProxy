namespace SupercellProxy.Networking.Protocol.ResourceAssociations;

/// Carries one ordered resource-association update and its provider labels.
public sealed record ResourceAssociationRecord(ResourceAssociationKey Key, string? SecondaryLabel, string? TertiaryLabel, string? QuaternaryLabel, string? DisplayName);
