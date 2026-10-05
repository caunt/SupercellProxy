namespace SupercellProxy.Networking.Transport;

/// <summary>Named result returned by ResolveAsync.</summary>
public readonly record struct ConnectionAddress(string Host, int Port);
