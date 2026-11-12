namespace SupercellProxy.Networking.Transport;

/// <summary>Named result returned by ResolveAsync.</summary>
/// <param name="Host">Resolved IPv4 address in dotted text form, or the literal IP address given on the command line, passed through unchanged.</param>
/// <param name="Port">TCP port to connect to, 9339 unless another number was given.</param>
public readonly record struct ConnectionAddress(string Host, int Port);
