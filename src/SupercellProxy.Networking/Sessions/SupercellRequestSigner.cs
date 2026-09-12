using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace SupercellProxy.Networking.Sessions;

/// <summary>
/// Defines the Scid Request Signing contract.
/// </summary>
public static class SupercellRequestSigner
{
    /// <summary>
    /// Provides the User Agent value or operation.
    /// </summary>
    public const string UserAgent =
        "scid/1.5.8-f (iOS 18.1; soil-prod; iPhone) com.supercell.hayday/1.72.84";
    private static readonly byte[] Key = DecodeKey();

    /// <summary>
    /// Provides the Sign value or operation.
    /// </summary>
    public static string Sign(string path, string body, string userAgent, string deviceIdentifier, string? authorization = null)
    {
        string timestamp = DateTimeOffset
            .UtcNow.ToUnixTimeSeconds()
            .ToString(CultureInfo.InvariantCulture);

        string value =
            timestamp
            + "POST"
            + path
            + body
            + (authorization is null ? string.Empty : "authorization=" + authorization)
            + "user-agent="
            + userAgent
            + "x-supercell-device-id="
            + deviceIdentifier;

        string signature = Convert
            .ToBase64String(HMACSHA256.HashData(Key, Encoding.UTF8.GetBytes(value)))
            .TrimEnd(trimChar: '=')
            .Replace(oldChar: '+', newChar: '-')
            .Replace(oldChar: '/', newChar: '_');

        return "RFPv1 Timestamp="
            + timestamp
            + ",SignedHeaders="
            + (authorization is null ? string.Empty : "authorization;")
            + "user-agent;x-supercell-device-id,Signature="
            + signature;
    }

    private static byte[] DecodeKey()
    {
        // Hay Day Apple signer 1017d97f8 and permutation helper 1017da930.
        byte[] encoded = Convert.FromHexString(s: "DD68010D2BEC62C63FE89D562C07673D6F15EA3EF033A2814D3251157DE55C46");

        int[] permutation = [.. Enumerable.Range(start: 0, encoded.Length)];
        uint state = 42;

        for (int index = permutation.Length - 1; index > 0; index--)
        {
            state = unchecked((state * 1_664_525) + 1_013_904_223);
            int swap = Convert.ToInt32(state % Convert.ToUInt32(index + 1));
            (permutation[index], permutation[swap]) = (permutation[swap], permutation[index]);
        }

        byte[] decoded = new byte[encoded.Length];

        for (int index = 0; index < encoded.Length; index++)
            decoded[permutation[index]] = encoded[index];

        return decoded;
    }
}
