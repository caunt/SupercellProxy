using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class CustomizationManagerState
{
    private GameRandom? selectionRandom;

    private CustomizationManagerState(bool builderAvailable)
    {
        BuilderAvailable = builderAvailable;
    }

    public bool BuilderAvailable { get; private set; }

    public static CustomizationManagerState Create(CustomizationManagerSnapshot? snapshot)
    {
        if (snapshot is null)
            throw new InvalidDataException("The saved state has no customization manager.");

        if (snapshot.StockSeconds is not 0)
        {
            throw new NotSupportedException(
                "Restoring active customization stock is not implemented."
            );
        }

        return new CustomizationManagerState(builderAvailable: false);
    }

    public void Update(GameRandom random)
    {
        ArgumentNullException.ThrowIfNull(random);

        if (selectionRandom is not null)
            return;

        selectionRandom = new GameRandom(random.NextInt(int.MaxValue));
        BuilderAvailable = true;
    }
}
