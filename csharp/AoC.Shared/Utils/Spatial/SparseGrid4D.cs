using System.Text;

namespace AoC.Shared.Utils.Spatial;

public class SparseGrid4D<T>
{
    public Dictionary<Coord4D, T> Cells = new();

    public T this[Coord4D cell]
    {
        get => Cells[cell];
        set => Cells[cell] = value;
    }

    public bool TryGetCell(Coord4D cell, out T value)
    {
        return Cells.TryGetValue(cell, out value);
    }
    
    public T GetValueOrDefault(Coord4D cell, T defaultValue)
    {
        return Cells.GetValueOrDefault(cell, defaultValue);
    }
    
    public (Coord4D Lower, Coord4D Upper) GetBounds()
    {
        if (Cells.Count == 0)
            return (new Coord4D(0, 0, 0, 0), new Coord4D(0, 0, 0, 0));

        int minX = int.MaxValue, minY = int.MaxValue, minZ = int.MaxValue, minW = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue, maxZ = int.MinValue, maxW = int.MinValue;

        foreach (var key in Cells.Keys)
        {
            minX = Math.Min(minX, key.X);
            minY = Math.Min(minY, key.Y);
            minZ = Math.Min(minZ, key.Z);
            minW = Math.Min(minW, key.W);
            maxX = Math.Max(maxX, key.X);
            maxY = Math.Max(maxY, key.Y);
            maxZ = Math.Max(maxZ, key.Z);
            maxW = Math.Max(maxW, key.W);
        }

        return (new Coord4D(minX, minY, minZ, minW), new Coord4D(maxX, maxY, maxZ, maxW));
    }
    
    public IEnumerable<T> GetNeighbors(Coord4D cell)
    {
        var (x, y, z, w) = cell;

        for (var dx = -1; dx <= 1; dx++)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                for (var dz = -1; dz <= 1; dz++)
                {
                    for (var dw = -1; dw <= 1; dw++)
                    {
                        if (dx == 0 && dy == 0 && dz == 0 && dw == 0)
                            continue;

                        var neighborCoord = new Coord4D(x + dx, y + dy, z + dz, w + dw);
                        
                        if (TryGetCell(neighborCoord, out var neighbor))
                            yield return neighbor;
                    }
                }
            }
        }
    }
    
    public IEnumerable<Coord4D> GetNeighborCells(Coord4D cell)
    {
        var (x, y, z, w) = cell;

        for (var dx = -1; dx <= 1; dx++)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                for (var dz = -1; dz <= 1; dz++)
                {
                    for (var dw = -1; dw <= 1; dw++)
                    {
                        if (dx == 0 && dy == 0 && dz == 0 && dw == 0)
                            continue;

                        yield return new Coord4D(x + dx, y + dy, z + dz, w + dw);
                    }
                }
            }
        }
    }

    public void Print(Func<T, string> cellToString)
    {
        var bounds = GetBounds();
        var sb = new StringBuilder();

        for (var w = bounds.Lower.W; w <= bounds.Upper.W; w++)
        {
            for (var z = bounds.Lower.Z; z <= bounds.Upper.Z; z++)
            {
                sb.AppendLine($"z={z} w={w}");
                for (var y = bounds.Lower.Y; y <= bounds.Upper.Y; y++)
                {
                    for (var x = bounds.Lower.X; x <= bounds.Upper.X; x++)
                    {
                        var coord = new Coord4D(x, y, z, w);
                        sb.Append(TryGetCell(coord, out var value) ? cellToString(value) : ".");
                    }
                    sb.AppendLine();
                }
                sb.AppendLine();
            }
            sb.AppendLine();
        }

        Console.Write(sb.ToString());
    }

}