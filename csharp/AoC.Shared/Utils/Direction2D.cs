namespace AoC.Shared.Utils;

public record Direction2D(int DeltaX, int DeltaY)
{
    public static Direction2D Up => new(0, -1);
    public static Direction2D Down => new(0, 1);
    public static Direction2D Left => new(-1, 0);
    public static Direction2D Right => new(1, 0);

    public static Direction2D UpLeft => new(-1, -1);
    public static Direction2D UpRight => new(1, -1);
    public static Direction2D DownLeft => new(-1, 1);
    public static Direction2D DownRight => new(1, 1);

    public static Direction2D[] CardinalDirections => new[]
    {
        Up, Down, Left, Right
    };

    public static Direction2D[] DiagonalDirections => new[]
    {
        UpLeft, UpRight, DownLeft, DownRight
    };

    public static Direction2D[] AllDirections => CardinalDirections.Concat(DiagonalDirections).ToArray();
    
    public Direction2D Rotate90ClockwiseForGrid() => new(-DeltaY, DeltaX);
}
