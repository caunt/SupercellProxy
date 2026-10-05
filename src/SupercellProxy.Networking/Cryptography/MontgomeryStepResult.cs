using System.Numerics;

namespace SupercellProxy.Networking.Cryptography;

/// <summary>Named result returned by MontgomeryStep.</summary>
internal readonly record struct MontgomeryStepResult(BigInteger XCoordinate2, BigInteger ZCoordinate2, BigInteger XCoordinate3, BigInteger ZCoordinate3);
