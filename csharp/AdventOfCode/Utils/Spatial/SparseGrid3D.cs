using System.Text;

namespace AdventOfCode.Utils.Spatial;

public class SparseGrid3D<T>
{
    public Dictionary<Coord3D, T> Cells = new();

    public T this[Coord3D cell]
    {
        get => Cells[cell];
        set => Cells[cell] = value;
    }

    public bool TryGetCell(Coord3D cell, out T value)
    {
        return Cells.TryGetValue(cell, out value);
    }
    
    public T GetValueOrDefault(Coord3D cell, T defaultValue)
    {
        return Cells.GetValueOrDefault(cell, defaultValue);
    }

    public IEnumerable<T> GetNeighbors(Coord3D cell)
    {
        var (x, y, z) = cell;
        
        for (var dx = -1; dx <= 1; dx++)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                for (var dz = -1; dz <= 1; dz++)
                {
                    if (dx == 0 && dy == 0 && dz == 0)
                        continue;

                    if (TryGetCell(new(x + dx, y + dy, z + dz), out var neighbor))
                        yield return neighbor;
                }
            }
        }
    }
    
    public IEnumerable<Coord3D> GetNeighborCells(Coord3D cell)
    {
        var (x, y, z) = cell;
        
        for (var dx = -1; dx <= 1; dx++)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                for (var dz = -1; dz <= 1; dz++)
                {
                    if (dx == 0 && dy == 0 && dz == 0)
                        continue;

                    yield return new Coord3D(x + dx, y + dy, z + dz);
                }
            }
        }
    }
    
    public (Coord3D Lower, Coord3D Upper) GetBounds()
    {
        if (Cells.Count == 0)
        {
            return (new Coord3D(0, 0, 0), new Coord3D(0, 0, 0));
        }

        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int minZ = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;
        int maxZ = int.MinValue;

        foreach (var key in Cells.Keys)
        {
            minX = Math.Min(minX, key.X);
            minY = Math.Min(minY, key.Y);
            minZ = Math.Min(minZ, key.Z);
            maxX = Math.Max(maxX, key.X);
            maxY = Math.Max(maxY, key.Y);
            maxZ = Math.Max(maxZ, key.Z);
        }

        return (new Coord3D(minX, minY, minZ), new Coord3D(maxX, maxY, maxZ));
    }
    
    public void Print(Func<T, string> cellToString)
    {
        var bounds = GetBounds();
        var sb = new StringBuilder();

        sb.AppendLine($"Grid bounds: ({bounds.Lower.X},{bounds.Lower.Y},{bounds.Lower.Z}) to ({bounds.Upper.X},{bounds.Upper.Y},{bounds.Upper.Z})");

        for (var z = bounds.Lower.Z; z <= bounds.Upper.Z; z++)
        {
            sb.AppendLine($"z={z}");
            for (var y = bounds.Lower.Y; y <= bounds.Upper.Y; y++)
            {
                for (var x = bounds.Lower.X; x <= bounds.Upper.X; x++)
                {
                    sb.Append(TryGetCell((x, y, z), out var value) ? cellToString(value) : "."); // Default representation for empty cells
                }
                sb.AppendLine();
            }
            sb.AppendLine();
        }

        Console.Write(sb.ToString());
    }
}