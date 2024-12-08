using AoC.Shared;
using AoC.Shared.DataStructures;
using AoC.Shared.Extensions;
using AoC.Shared.Utils;
using Snapshooter.Extensions;
using Xunit;
using Xunit.Abstractions;
using Position = (int x, int y);
using AntennaDict = System.Collections.Generic.Dictionary<char, System.Collections.Generic.List<(int x, int y)>>;

namespace AoC._2024;

public class Day8(ITestOutputHelper testOutputHelper) : SolutionBase(2024, 8, false, testOutputHelper)
{
    public (CharGrid2D grid, AntennaDict antennas) Parse(string input)
    {
        var grid = new CharGrid2D(input);
        var symbols = input.ToCharArray().Distinct().Where(char.IsAsciiLetterOrDigit).ToArray();

        var antennas = new AntennaDict();
        foreach (var symbol in symbols)
        {
            antennas[symbol] = grid.FindAll(symbol);
        }

        return (grid, antennas);
    }
    
    public override async Task<Answer> Part1(string input)
    {
        var (grid, antennas) = Parse(input);
        var antinodes = new HashSet<Position>();
        
        // Triple for loop go brrrr
        foreach (var (_, positions) in antennas)
        {
            foreach (var main in positions)
            {
                foreach (var secondary in positions)
                {
                    if (main == secondary)
                        continue;

                    // Get the vector from main to secondary
                    Position vector = (secondary.x - main.x, secondary.y - main.y);

                    // Flip the vector and add it to the main position
                    Position antinode = (main.x - vector.x, main.y - vector.y);


                    if (grid.IsInBounds(antinode.x, antinode.y))
                        antinodes.Add(antinode);
                }
            }
        }

        return antinodes.Count;
    }

    public override async Task<Answer> Part2(string input)
    {
        var (grid, antennas) = Parse(input);
        var antinodes = new HashSet<Position>();
        
        // Triple for loop go brrrr
        foreach (var (_, positions) in antennas)
        {
            foreach (var main in positions)
            {
                antinodes.Add(main);
                foreach (var secondary in positions)
                {
                    if (main == secondary)
                        continue;

                    // Get the vector from main to secondary
                    Position vector = (secondary.x - main.x, secondary.y - main.y);

                    // Flip the vector and add it to the main position
                    Position antinode = (main.x - vector.x, main.y - vector.y);

                    while (grid.IsInBounds(antinode.x, antinode.y))
                    {
                        antinodes.Add(antinode);
                        
                        antinode = (antinode.x - vector.x, antinode.y - vector.y);  
                    }
                }
            }
        }

        return antinodes.Count;
    }
}