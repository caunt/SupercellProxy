namespace SupercellProxy.Playground.Home;

internal sealed class AnimalHabitatGridCell(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public bool Blocked { get; set; }
    public bool Walkable { get; set; } = true;
    public GameObjectState? Occupant { get; set; }
}
