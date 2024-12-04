using AdventOfCode.Extensions;
using AdventOfCode.Utils.Spatial;
using Spectre.Console;

namespace AdventOfCode._2024;

[EventIdentifier(2024, 4)]
public class Day4 : Solution
{
    public override Answer Part1(string input)
    {
        var grid = new CharGrid2D(input.ToUpper());

        var xes = grid.FindAll('X');

        var directions = new (int dx, int dy)[]
        {
            (1, 0), // right
            (0, 1), // down
            (1, 1), // down right
            (1, -1), // up right
            (-1, 0), // left
            (0, -1), // up
            (-1, -1), // up left
            (-1, 1), // down left
        };

        var sum = 0;
        foreach (var (x, y) in xes)
        {
            foreach (var (dx, dy) in directions)
            {
                var word = grid.GetVector(x, y, dx, dy, 4);

                if (word == "XMAS")
                {
                    sum++;
                }
            }
        }

        return sum;
    }

    public override Answer Part2(string input)
    {
        var grid = new CharGrid2D(input.ToUpper());
        var ms = grid.FindAll('A');

        var directions = new (int dx, int dy)[]
        {
            (1, 1), // down right
            (1, -1), // up right
            (-1, -1), // up left
            (-1, 1), // down left
        };

        var sum = 0;
        foreach (var (x, y) in ms)
        {
            var words = new List<string>();
            foreach (var (dx, dy) in directions)
            {
                var word = grid.GetVector(x - dx, y - dy, dx, dy, 3);

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