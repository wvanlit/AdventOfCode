using AoC.Shared;
using AoC.Shared.DataStructures;
using AoC.Shared.Utils;
using AoC.Shared.Utils.Spatial;
using Xunit.Abstractions;

namespace AoC._2024;

public class Day4(ITestOutputHelper testOutputHelper) : SolutionBase(2024, 4, false, testOutputHelper)
{
    public override async Task<Answer> Part1(string input)
    {
        var grid = new CharGrid2D(input.ToUpper());

        var xes = grid.FindAll('X');

        var sum = 0;
        foreach (var (x, y) in xes)
        {
            foreach (var dir in Direction2D.AllDirections)
            {
                var word = grid.GetVector(x, y, dir, 4);

                if (word == "XMAS")
                {
                    sum++;
                }
            }
        }

        return sum;
    }

    public override async Task<Answer> Part2(string input)
    {
        var grid = new CharGrid2D(input.ToUpper());
        var ms = grid.FindAll('A');

        var sum = 0;
        foreach (var (x, y) in ms)
        {
            var words = new List<string>();
            foreach (var dir in Direction2D.DiagonalDirections)
            {
                var word = grid.GetVector(x - dir.DeltaX, y - dir.DeltaY, dir, 3);

                if (word == "MAS")
                {
                    words.Add(word);
                }
            }
            
            if (words.Count == 2)
            {
                sum++;
            }
        }

        return sum;
    }
}