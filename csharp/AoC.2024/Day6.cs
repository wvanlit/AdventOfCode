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
    const char Path = 'X';


    public override async Task<Answer> Part1(string input)
    {
        var grid = new CharGrid2D(input);
        var path = new CharGrid2D(grid, Empty);

        var obstructions = grid.FindAll(Obstruction);
        var guardPos = grid.FindAll(Guard).Single();

        path.Grid[guardPos.y][guardPos.x] = Path;

        // Guard Rules:
        // 1. If there is something directly in front of you, turn right 90 degrees
        // 2. Otherwise, take a step forward.
        // 3. Leaving the grid means end of patrol
        var dir = Direction2D.Up;

        while (grid.IsInBounds(
                   guardPos.x + dir.DeltaX,
                   guardPos.y + dir.DeltaY))
        {
            var nx = guardPos.x + dir.DeltaX;
            var ny = guardPos.y + dir.DeltaY;

            var isObstructed = obstructions.Any(o => o.x == nx && o.y == ny);

            if (isObstructed)
            {
                dir = dir.Rotate90ClockwiseForGrid();
            }
            else
            {
                guardPos = (nx, ny);
                path.Grid[ny][nx] = Path;
            }
        }

        obstructions.ForEach(o => path.Grid[o.y][o.x] = Obstruction);

        return path.FindAll(Path).Count;
    }

    public bool GridIsInfiniteLoop(CharGrid2D grid)
    {
        var path = new HashSet<(Direction2D, int x, int y)>();

        var dir = Direction2D.Up;
        var obstructions = grid.FindAll(Obstruction);
        var guardPos = grid.FindAll(Guard).Single();

        path.Add((dir, guardPos.x, guardPos.y));

        // Guard Rules:
        // 1. If there is something directly in front of you, turn right 90 degrees
        // 2. Otherwise, take a step forward.
        // 3. Leaving the grid means end of patrol

        while (grid.IsInBounds(
                   guardPos.x + dir.DeltaX,
                   guardPos.y + dir.DeltaY))
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
                return true;
            }
            else
            {
                guardPos = (nx, ny);
                path.Add((dir, nx, ny));
            }
        }

        // If we break out of the loop, we have reached the end of the grid
        // Thus, the grid is not an infinite loop
        return false;
    }

    public override async Task<Answer> Part2(string input)
    {
        var grid = new CharGrid2D(input);
        var emptySpots = grid.FindAll(Empty).ToArray();

        var tasks = new List<Task<int>>();
        
        OutputHelper.WriteLine($"Empty Spots: {emptySpots.Length}");
        
        foreach (var emptySpot in emptySpots)
        {
            var clone = new CharGrid2D(grid);
            clone.Grid[emptySpot.y][emptySpot.x] = Obstruction;

            tasks.Add(Task.Run(() => GridIsInfiniteLoop(clone) ? 1 : 0));
        }
        
        var sum = 0;
        foreach (var task in tasks)
        {
            sum += await task;
        }

        return sum;
    }
}