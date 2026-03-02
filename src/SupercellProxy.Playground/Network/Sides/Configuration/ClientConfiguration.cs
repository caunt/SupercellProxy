namespace SupercellProxy.Playground.Network.Sides.Configuration;

public record ClientConfiguration(string UpstreamHost, int UpstreamPort, ProtocolConfiguration Protocol);
