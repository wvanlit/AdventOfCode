namespace AoC.Shared.Utils.Spatial;

public class ArrayGrid2D<T>
{
    private readonly T[,] _grid;
    public int Width { get; }
    public int Height { get; }

    public ArrayGrid2D(int width, int height)
    {
        Width = width;
        Height = height;
        _grid = new T[width, height];
    }
    
    public T this[Coord2D cell]
    {
        get => GetValueOrDefault(cell);
        set => SetCell(cell, value);
    }

    public bool TryGetCell(Coord2D coord, out T value)
    {
        if (IsWithinBounds(coord))
        {
            value = _grid[coord.X, coord.Y];
            return true;
        }

        value = default!;
        return false;
    }

    public T GetValueOrDefault(Coord2D coord, T defaultValue = default)
    {
        return IsWithinBounds(coord) ? _grid[coord.X, coord.Y] : defaultValue;
    }

    public void SetCell(Coord2D coord, T value)
    {
        if (IsWithinBounds(coord))
        {
            _grid[coord.X, coord.Y] = value;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(coord), "Coordinate is out of grid bounds.");
        }
    }

    public void Print(Func<T, string> cellToString)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Console.Write(cellToString(_grid[x, y]) + " ");
            }
            Console.WriteLine();
        }
    }

    private bool IsWithinBounds(Coord2D coord)
    {
        return coord.X >= 0 && coord.X < Width && coord.Y >= 0 && coord.Y < Height;
    }
    
    public T[] GetRow(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= Height)
        {
            throw new ArgumentOutOfRangeException(nameof(rowIndex), "Row index is out of bounds.");
        }

        T[] row = new T[Width];
        for (int x = 0; x < Width; x++)
        {
            row[x] = _grid[x, rowIndex];
        }

        return row;
    }

    public T[] GetColumn(int columnIndex)
    {
        if (columnIndex < 0 || columnIndex >= Width)
        {
            throw new ArgumentOutOfRangeException(nameof(columnIndex), "Column index is out of bounds.");
        }

        T[] column = new T[Height];
        for (int y = 0; y < Height; y++)
        {
            column[y] = _grid[columnIndex, y];
        }

        return column;
    }
}