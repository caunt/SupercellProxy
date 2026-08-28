namespace SupercellProxy.Playground.Home.Checksum;

internal static class GameModeChecksum
{
    public static TurnChecksum Calculate(HomeState state)
    {
        var encoder = new ChecksumEncoder();
        encoder.WriteBoolean(HomeState.ChecksumEnabled);

        if (!HomeState.ChecksumEnabled)
            return new TurnChecksum(encoder.Checksum, new int[8]);

        encoder.WriteVarInt(state.ServerTimestamp);
        encoder.WriteVarInt(HomeState.GameMode);
        encoder.WriteVarInt(state.Tick.SubTick);
        encoder.WriteBoolean(HomeState.FullChecksumEnabled);
        encoder.WriteBoolean(HomeState.DebugChecksumEnabled);

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

        if (HomeState.FullChecksumEnabled || HomeState.DebugChecksumEnabled)
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

    public static void EncodeTick(ChecksumEncoder encoder, HomeState state)
    {
        encoder.WriteVarInt(state.Tick.SubTick);
        encoder.WriteVarInt(state.Tick.Tick);
    }

    public static void EncodeRandom(ChecksumEncoder encoder, HomeState state)
    {
        encoder.WriteInt32(state.Random.State);
    }
}
