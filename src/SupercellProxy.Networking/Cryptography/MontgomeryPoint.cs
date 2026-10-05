using System.Numerics;

namespace SupercellProxy.Networking.Cryptography;

/// <summary>Named result returned by RunMontgomeryLadder.</summary>
internal readonly record struct MontgomeryPoint(BigInteger XCoordinate, BigInteger ZCoordinate);
