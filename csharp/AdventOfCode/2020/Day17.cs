using AdventOfCode.Extensions;
using AdventOfCode.Utils.Spatial;
using MoreLinq;

namespace AdventOfCode._2020;

[EventIdentifier(2020, 17)]
public class Day17 : Solution
{
    public SparseGrid3D<bool> Parse3D(string input)
    {
        var grid = new SparseGrid3D<bool>();

        foreach (var (i, row) in input.SplitLines().Index())
        {
            foreach (var (j, cell) in row.ToCharArray().Index())
            {
                grid[(j, i, 0)] = cell == '#';
            }
        }

        return grid;
    }
    
    public SparseGrid4D<bool> Parse4D(string input)
    {
        var grid = new SparseGrid4D<bool>();

        foreach (var (i, row) in input.SplitLines().Index())
        {
            foreach (var (j, cell) in row.ToCharArray().Index())
            {
                grid[(j, i, 0, 0)] = cell == '#';
            }
        }

        return grid;
    }


    public override Answer Part1(string input)
    {
        const int totalCycles = 6;
        var grid = Parse3D(input);

        for (var cycle = 0; cycle < totalCycles; cycle++)
        {
            // AnsiConsole.WriteLine($"After {cycle} cycles");
            // grid.Print(c => c ? "#" : ".");
            
            var cellsToCheck = grid.Cells.Keys.SelectMany(cell => grid.GetNeighborCells(cell)).Distinct();
            var updates = new List<(Coord3D, bool)>();
            
            foreach (var cell in cellsToCheck)
            {
                var neighbors = grid.GetNeighbors(cell);
                var neighborCount = neighbors.Count(n => n == true);
                var cellState = grid.GetValueOrDefault(cell, false);

                switch (cellState)
                {
                    case true when neighborCount is 2:
                    case true when neighborCount is 3:
                        // If a cube is active and exactly 2 or 3 of its neighbors are also active,
                        // the cube remains active.
                        break;
                    case true:
                        // Otherwise, the cube becomes inactive.
                        updates.Add((cell, false));
                        break;
                    case false when neighborCount == 3:
                        // If a cube is inactive but exactly 3 of its neighbors are active,
                        // the cube becomes active.
                        updates.Add((cell, true));
                        break;
                }
            }

            foreach (var update in updates)
            {
                grid[update.Item1] = update.Item2;
            }
        }

        return grid.Cells.Values.Count(c => c != false);
    }

    public override Answer Part2(string input)
    {
        const int totalCycles = 6;
        var grid = Parse4D(input);

        for (var cycle = 0; cycle < totalCycles; cycle++)
        {
            // AnsiConsole.WriteLine($"After {cycle} cycles");
            // grid.Print(c => c ? "#" : ".");
            
            var cellsToCheck = grid.Cells.Keys.SelectMany(cell => grid.GetNeighborCells(cell)).Distinct();
            var updates = new List<(Coord4D, bool)>();
            
            foreach (var cell in cellsToCheck)
            {
                var neighbors = grid.GetNeighbors(cell);
                var neighborCount = neighbors.Count(n => n == true);
                var cellState = grid.GetValueOrDefault(cell, false);

                switch (cellState)
                {
                    case true when neighborCount is 2:
                    case true when neighborCount is 3:
                        // If a cube is active and exactly 2 or 3 of its neighbors are also active,
                        // the cube remains active.
                        break;
                    case true:
                        // Otherwise, the cube becomes inactive.
                        updates.Add((cell, false));
                        break;
                    case false when neighborCount == 3:
                        // If a cube is inactive but exactly 3 of its neighbors are active,
                        // the cube becomes active.
                        updates.Add((cell, true));
                        break;
                }
            }

            foreach (var update in updates)
            {
                grid[update.Item1] = update.Item2;
            }
        }

        return grid.Cells.Values.Count(c => c != false);
    }
}