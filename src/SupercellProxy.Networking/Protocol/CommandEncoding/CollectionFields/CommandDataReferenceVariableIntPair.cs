using System.Runtime.InteropServices;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandDataReferenceVarIntPair</c>.
/// </summary>
/// <param name="GlobalIdentifier">The referenced data global ID.</param>
/// <param name="Value">The associated variable-length integer.</param>
[StructLayout(LayoutKind.Auto)]
public readonly record struct CommandDataReferenceVariableIntPair([property: System.Text.Json.Serialization.JsonPropertyName("GlobalId")] int GlobalIdentifier, int Value);
