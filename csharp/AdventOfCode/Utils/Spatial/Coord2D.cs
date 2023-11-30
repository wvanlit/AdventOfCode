namespace AdventOfCode.Utils.Spatial;

public record struct Coord2D(int X, int Y)
{
    public static implicit operator Coord2D((int X, int Y) tuple) => new(tuple.X, tuple.Y);
};