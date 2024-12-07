using AoC.Shared;
using AoC.Shared.DataStructures;
using AoC.Shared.Extensions;
using AoC.Shared.Utils;
using Snapshooter.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace AoC._2024;

using RuleDict = Dictionary<int, int[]>;

public class Day6(ITestOutputHelper testOutputHelper) : SolutionBase(2024, 6, false, testOutputHelper)
{
    const char Obstruction = '#';
    const char Guard = '^';
    const char Empty = '.';

    private (bool infinite, int distinctPath, List<(int x, int y)> path) TravelGrid(CharGrid2D grid)
    {
        var path = new HashSet<(Direction2D, int x, int y)>();

        var dir = Direction2D.Up;
        var obstructions = grid.FindAll(Obstruction);
        var guardPos = grid.FindAll(Guard).Single();

        path.Add((dir, guardPos.x, guardPos.y)); // Add the starting position to the path

        // Guard Rules:
        // 1. If there is something directly in front of you, turn right 90 degrees
        // 2. Otherwise, take a step forward.
        // 3. Leaving the grid means end of patrol
        while (grid.IsInBounds(guardPos.x + dir.DeltaX, guardPos.y + dir.DeltaY))
        {
            var nx = guardPos.x + dir.DeltaX;
            var ny = guardPos.y + dir.DeltaY;

            var isObstructed = obstructions.Any(o => o.x == nx && o.y == ny);

            if (isObstructed)
            {
                dir = dir.Rotate90ClockwiseForGrid();
            }
            else if (path.Contains((dir, nx, ny)))
            {
                return (true, -1, null!); // Infinite loop
            }
            else
            {
                guardPos = (nx, ny);
                path.Add((dir, nx, ny));
            }
        }

        // If we break out of the loop, we have reached the end of the grid
        // Thus, the grid is not an infinite loop
        var distinctPath = path.Select(p => (p.x, p.y)).Distinct().ToList();
        return (false, distinctPath.Count, distinctPath);
    }

    public override async Task<Answer> Part1(string input)
    {
        return TravelGrid(new CharGrid2D(input)).distinctPath;
    }

    public override async Task<Answer> Part2(string input)
    {
        var grid = new CharGrid2D(input);

        var emptySpots = TravelGrid(grid).path.Where(p => grid.Grid[p.y][p.x] == Empty).ToList();

        OutputHelper.WriteLine($"Empty Spots: {emptySpots.Count}");

        var sum = 0;
        Parallel.ForEach(emptySpots, t =>
        {
            var clone = new CharGrid2D(grid);
            clone.Grid[t.y][t.x] = Obstruction; // Block the empty spot

            if (TravelGrid(clone).infinite)
            {
                Interlocked.Increment(ref sum);
            }
        });
        
        return sum;
    }
}