using System.Globalization;
using System.Text.Json;

using SupercellProxy.Networking.Protocol.Fishing.Molluscs;

namespace SupercellProxy.Networking.Protocol.GameObjects;

public sealed partial record GameObjectSnapshot
{
    /// <summary>
    /// Reads one location of a mussel bed from the bed's numbered members, LocationUnlocked0, LocationMolluscGlobalId0 and so on; a
    /// location the save leaves out is locked and has no mussel.
    /// </summary>
    /// <param name="index">The location, starting at 0.</param>
    public MolluscBedLocationSnapshot MolluscBedLocation(int index)
    {
        string suffix = index.ToString(CultureInfo.InvariantCulture);
        bool unlocked = ExtensionData.TryGetValue("LocationUnlocked" + suffix, out JsonElement flag) && flag.ValueKind == JsonValueKind.True;
        bool linked = ExtensionData.TryGetValue("LocationMolluscGlobalId" + suffix, out JsonElement mollusc) && mollusc.ValueKind == JsonValueKind.Number;

        return new(unlocked, linked && mollusc.TryGetInt32(out int globalId) ? globalId : 0);
    }
}
