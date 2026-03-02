namespace SupercellProxy.Playground.Network.Sides.Configuration;

public record ProxyConfiguration(string UpstreamHost, int UpstreamPort, string ListenAddress, int ListenPort, ProtocolConfiguration Protocol);
