using System.Text.RegularExpressions;
using AoC.Shared;
using AoC.Shared.DataStructures;
using AoC.Shared.Extensions;
using AoC.Shared.Utils;
using Xunit;
using Xunit.Abstractions;
using static System.Math;

namespace AoC._2025;

public class Day4(ITestOutputHelper testOutputHelper) : SolutionBase(2025, 4, false, testOutputHelper)
{
    const char RollChar = '@';
    const char EmptyChar = '.';
    
    public override async Task<Answer> Part1(string input)
    {
        var grid = new CharGrid2D(input);
        
        var rollsAccessible = 0;
        grid.ForEach(cell =>
        {
            if (cell.Char != RollChar) return;

            var rolls = cell.Neighbors(Direction2D.AllDirections, grid).Count(c => c.Char == RollChar);
            if(rolls < 4) rollsAccessible++;
        });

        return rollsAccessible;
    }

    public override async Task<Answer> Part2(string input)
    {
        var grid = new CharGrid2D(input);
        
        var rollsAccessible = 0;
        var previousAccessible = 0;

        do
        {
            previousAccessible = rollsAccessible;
            
            grid.ForEach(cell =>
            {
                if (cell.Char != RollChar) return;

                var rolls = cell.Neighbors(Direction2D.AllDirections, grid).Count(c => c.Char == RollChar);

                if (rolls < 4)
                {
                    rollsAccessible++;
                    // Remove the roll from the grid
                    grid.Grid[cell.Y][cell.X] = EmptyChar;
                }
            });
        } while (rollsAccessible != previousAccessible);

        return rollsAccessible;
    }   
}