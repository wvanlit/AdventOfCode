using System.Text;
using AoC.Shared.Extensions;
using AoC.Shared.Utils;
using Spectre.Console;
using Xunit.Abstractions;

namespace AoC.Shared.DataStructures;

public class CharGrid2D
{
    public char[][] Grid { get; }

    public int Height => Grid.Length;
    public int Width => Grid[0].Length;

    public CharGrid2D(string input)
    {
        Grid = input.SplitLines().Select(s => s.ToCharArray()).ToArray();
    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    public CharGrid2D(CharGrid2D other, char? fill = null)
    {
        Grid = new char[other.Height][];

        for (var y = 0; y < other.Height; y++)
        {
            Grid[y] = new char[other.Width];
            for (var x = 0; x < other.Width; x++)
            {
                Grid[y][x] = fill ?? other.Grid[y][x];
            }
        }
    }

    public void Print(ITestOutputHelper outputHelper)
    {
        var sb = new StringBuilder();
        foreach (var row in Grid)
        {
            foreach (var col in row)
            {
                sb.Append(col);
            }

            sb.AppendLine();
        }

        outputHelper.WriteLine(sb.ToString());
    }

    public Cell? At(int x, int y)
    {
        if (!IsInBounds(x, y)) return null;

        try
        {
            return new Cell(x, y, Grid[y][x]);
        }
        catch (IndexOutOfRangeException e)
        {
            return null;
        }
    }

    public void ForEach(Action<Cell> action)
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                action(At(x, y)!);
            }
        }
    }

    public List<(int x, int y)> FindAll(char value)
    {
        var result = new List<(int, int)>();

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (Grid[y][x] == value)
                {
                    result.Add((x, y));
                }
            }
        }

        return result;
    }

    public string GetVector(int x, int y, Direction2D dir, int len)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < len; i++)
        {
            var nx = x + dir.DeltaX * i;
            var ny = y + dir.DeltaY * i;

            if (nx < 0 || nx >= Width || ny < 0 || ny >= Height)
            {
                return sb.ToString();
            }

            sb.Append(Grid[ny][nx]);
        }

        return sb.ToString();
    }

    public bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    public sealed record Cell(int X, int Y, char Char)
    {
        public List<Cell> Neighbors(IEnumerable<Direction2D> directions, CharGrid2D grid) =>
            directions
                .Select(d => grid.At(X + d.DeltaX, Y + d.DeltaY))
                .Where(c => c != null)
                .Cast<Cell>()
                .ToList();
        
        public override string ToString() => $"{Char} ({X}, {Y})";
    }
}