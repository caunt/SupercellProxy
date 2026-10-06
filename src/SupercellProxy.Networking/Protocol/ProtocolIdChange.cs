namespace SupercellProxy.Networking.Protocol;

// A null id removes the message from this release onward; a later change can register it again.
internal sealed record ProtocolIdChange(Version Since, int? Id);
