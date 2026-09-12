using System.Text.Json.Serialization;

using SupercellProxy.Keys.Svelte;

namespace SupercellProxy.Keys.DecryptDay;

[JsonSerializable(typeof(DecryptDayPageResponse))]
[JsonSerializable(typeof(DecryptDayFileMetadata))]
[JsonSerializable(typeof(DecryptDayFileList))]
[JsonSerializable(typeof(DecryptDayPageNode))]
[JsonSerializable(typeof(DecryptDayFilePayload))]
[JsonSerializable(typeof(DecryptDayFileEnvelope))]
[JsonSerializable(typeof(DecryptDayApplicationPayload))]
[JsonSerializable(typeof(DecryptDayApplicationMetadata))]
[JsonSerializable(typeof(DecryptDayVersionMetadata))]
[JsonSerializable(typeof(SvelteData))]
internal sealed partial class DecryptDaySerializationContext : JsonSerializerContext;
