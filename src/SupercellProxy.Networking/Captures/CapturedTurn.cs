namespace SupercellProxy.Networking.Captures;

/// <summary>The native client turn recorded with a captured frame.</summary>
/// <param name="SubTick">The turn's sub-tick.</param>
/// <param name="Checksum">The turn checksum.</param>
/// <param name="SubChecksums">The per-lane checksums.</param>
/// <param name="Commands">The command identifiers in execution order.</param>
/// <param name="ExecutionPhases">The execution phase of each command.</param>
/// <param name="UndecodedCommandCount">The number of commands that could not be decoded, when any were missing.</param>
public sealed record CapturedTurn(int SubTick, int Checksum, int[] SubChecksums, int[] Commands, int[] ExecutionPhases, int? UndecodedCommandCount = null);
