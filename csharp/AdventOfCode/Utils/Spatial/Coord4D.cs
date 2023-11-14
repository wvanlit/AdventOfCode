namespace AdventOfCode.Utils.Spatial;

public record struct Coord4D(int X, int Y, int Z, int W)
{
    public static implicit operator Coord4D((int X, int Y, int Z, int W) tuple) 
        => new(tuple.X, tuple.Y, tuple.Z, tuple.W);
};