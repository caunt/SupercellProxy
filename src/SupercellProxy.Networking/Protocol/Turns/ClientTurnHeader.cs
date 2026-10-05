namespace SupercellProxy.Networking.Protocol.Turns;

/// <summary>Named result returned by ReadHeader.</summary>
public readonly record struct ClientTurnHeader(int Checksum, int SubTick, int[] SubChecksums, int CommandCount);
