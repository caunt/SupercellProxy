namespace SupercellProxy.Playground.Supercell;

/// <summary>
/// Runtime environments encoded by the native Hay Day client.
/// </summary>
public enum LogicEnvironment
{
    Development = 0,
    Stage = 1,
    Unknown = 2,
    Production = 3,
    LoadTest = 4,
    ProductionStage = 5,
    ProductionStageExternal = 6
}
