namespace AoC.Shared.Utils.Spatial;

public record struct Coord3D(int X, int Y, int Z)
{
    public static implicit operator Coord3D((int X, int Y, int Z) tuple) => new(tuple.X, tuple.Y, tuple.Z);
};