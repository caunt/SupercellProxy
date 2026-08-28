namespace SupercellProxy.Playground.Home.Checksum;

internal static class ExpansionReadyDataChecksum
{
    public static void Encode(ChecksumEncoder encoder, HomeState state)
    {
        encoder.WriteVarInt(state.ExpansionReadyDatas.Length);

        foreach (var expansionReadyData in state.ExpansionReadyDatas)
        {
            encoder.WriteVarInt(expansionReadyData.ExpansionData.GlobalId);
            encoder.WriteVarInt(expansionReadyData.ReadyBits);
        }
    }
}
