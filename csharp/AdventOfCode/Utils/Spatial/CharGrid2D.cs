using System.Text;
using AdventOfCode.Extensions;
using Spectre.Console;

namespace AdventOfCode.Utils.Spatial;

public class CharGrid2D
{
    public char[][] Grid { get; }
    
    public int Height => Grid.Length;
    public int Width => Grid[0].Length;
    
    public CharGrid2D(string input)
    {
        Grid = input.SplitLines().Select(s => s.ToCharArray()).ToArray();
    }

    public void Print()
    {
        foreach (var row in Grid)
        {
            foreach (var col in row)
            {
                AnsiConsole.Write(col);
            }
            AnsiConsole.WriteLine();
        }
    }
    
    public List<(int, int)> FindAll(char value)
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

    public string GetVector(int x, int y, int dx, int dy, int len)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < len; i++)
        {
            var nx = x + dx * i;
            var ny = y + dy * i;
            if (nx < 0 || nx >= Width || ny < 0 || ny >= Height)
            {
                return sb.ToString();
            }
            sb.Append(Grid[ny][nx]);
        }

        return sb.ToString();
    }
}