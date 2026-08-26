using SupercellProxy.Playground.Home.Simulation;

namespace SupercellProxy.Playground.Home.Checksum;

internal static class GameModeChecksum
{
    public static TurnChecksum Calculate(HarvestState state)
    {
        var encoder = new ChecksumEncoder();
        encoder.WriteBoolean(HarvestState.ChecksumEnabled);

        if (!HarvestState.ChecksumEnabled)
            return new TurnChecksum(encoder.Checksum, new int[8]);

        encoder.WriteVarInt(state.ServerTimestamp);
        encoder.WriteVarInt(HarvestState.GameMode);
        encoder.WriteVarInt(state.Tick.SubTick);
        encoder.WriteBoolean(HarvestState.FullChecksumEnabled);
        encoder.WriteBoolean(HarvestState.DebugChecksumEnabled);

        var subChecksums = new int[8];
        subChecksums[0] = encoder.Checksum;
        encoder.Reset();

        EncodeTick(encoder, state);
        subChecksums[1] = encoder.Checksum;
        encoder.Reset();

        EncodeRandom(encoder, state);
        subChecksums[2] = encoder.Checksum;
        encoder.Reset();

        ClientAvatarChecksum.EncodeAbbreviated(encoder, state);
        subChecksums[3] = encoder.Checksum;
        encoder.Reset();

        if (HarvestState.FullChecksumEnabled || HarvestState.DebugChecksumEnabled)
            throw new InvalidOperationException(
                "The enabled full/debug checksum path is not implemented."
            );

        GameObjectManagerChecksum.EncodeSecondary(encoder, state);
        subChecksums[5] = encoder.Checksum;
        encoder.Reset();

        ExpansionReadyDataChecksum.Encode(encoder, state);
        subChecksums[6] = encoder.Checksum;
        encoder.Reset();

        subChecksums[7] = state.CommandExecution.ExecutedCommandCount;

        foreach (var subChecksum in subChecksums)
            encoder.WriteInt32(subChecksum);

        return new TurnChecksum(encoder.Checksum, subChecksums);
    }

    public static void EncodeTick(ChecksumEncoder encoder, HarvestState state)
    {
        encoder.WriteVarInt(state.Tick.SubTick);
        encoder.WriteVarInt(state.Tick.Tick);
    }

    public static void EncodeRandom(ChecksumEncoder encoder, HarvestState state)
    {
        encoder.WriteInt32(state.Random.State);
    }
}
