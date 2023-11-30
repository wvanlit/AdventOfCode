using AdventOfCode.Extensions;
using AdventOfCode.Utils.Spatial;
using Spectre.Console;

namespace AdventOfCode._2020;

[EventIdentifier(2020, 20)]
public class Day20 : Solution
{
    record TileEdges(int Id, int[] Top, int[] Bottom, int[] Left, int[] Right)
    {
        public TileEdges(int Id, ArrayGrid2D<int> grid) :
            this(Id,
                Top: grid.GetRow(0),
                Bottom: grid.GetRow(9).Reverse().ToArray(),
                Left: grid.GetColumn(0).Reverse().ToArray(),
                Right: grid.GetColumn(9)) { }

        public void Print()
        {
            var grid = new Grid();
            grid.AddColumns(10);

            grid.AddRow(Top.Select(t => t.ToString()).ToArray());

            for (var i = 1; i < 10; i++)
            {
                grid.AddRow(Left[i].ToString(), "", "", "", "", "", "", "", "", Right[i].ToString());
            }

            grid.AddRow(Bottom.Select(t => t.ToString()).ToArray());

            AnsiConsole.Write(grid);
        }

        public TileEdges RotateRight()
        {
            return new TileEdges(
                Id,
                Top: Left,
                Right: Top,
                Bottom: Right,
                Left: Bottom);
        }
    }

    private (int id, ArrayGrid2D<int> grid) Parse(string tile)
    {
        var grid = new ArrayGrid2D<int>(10, 10);
        var parts = tile.Split(":");

        var lines = parts[1].Trim().Split("\n").ToArray();

        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y].ToCharArray();
            for (var x = 0; x < line.Length; x++)
            {
                grid[(x, y)] = line[x] == '#' ? 1 : 0;
            }
        }

        return (parts[0].Replace("Tile", "").ToInt(), grid);
    }

    public override Answer Part1(string input)
    {
        var tiles = input
            .Split("\n\n")
            .Select(Parse)
            .Select(tuple => new TileEdges(tuple.id, tuple.grid))
            .ToArray();

        foreach (var tile in tiles)
        {
            AnsiConsole.WriteLine("Tile: " + tile.Id);
            tile.Print();
            tile.RotateRight().Print();
            tile.RotateRight().RotateRight().Print();
            tile.RotateRight().RotateRight().RotateRight().Print();

            break;
        }

        throw new NotImplementedException();
    }

    public override Answer Part2(string input)
    {
        throw new NotImplementedException();
    }
}