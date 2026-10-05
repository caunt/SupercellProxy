using System.Runtime.InteropServices;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandDataReferenceVarIntPair</c>.
/// </summary>
/// <param name="GlobalId">The referenced data global ID.</param>
/// <param name="Value">The associated var-length integer.</param>
[StructLayout(LayoutKind.Auto)]
public readonly record struct CommandDataReferenceVarIntPair([property: System.Text.Json.Serialization.JsonPropertyName("GlobalId")] int GlobalId, int Value);
