using AoC.Shared;
using AoC.Shared.DataStructures;
using AoC.Shared.Utils;
using MoreLinq;
using Xunit;
using Xunit.Abstractions;
using static AoC.Shared.DataStructures.CharGrid2D;
using Region = System.Collections.Generic.List<AoC.Shared.DataStructures.CharGrid2D.Cell>;

namespace AoC._2024;

public class Day12(ITestOutputHelper outputHelper) :
    SolutionBase(year: 2024, day: 12, runTest: true, outputHelper)
{
    private List<Region> Regions(CharGrid2D grid)
    {
        var segmentedRegions = new Dictionary<char, List<Region>>(); // char -> list of regions

        // Initialize regions
        var set = grid.Grid.SelectMany(c => c).ToHashSet();
        set.ForEach(c => segmentedRegions[c] = []);

        // Find regions
        grid.ForEach(cell =>
        {
            var sameCharNeighbors = cell.Neighbors(Direction2D.CardinalDirections, grid)
                .Where(nc => nc.Char == cell.Char).ToList();
            var possibleRegions = segmentedRegions[cell.Char];

            if (sameCharNeighbors.Count == 0)
            {
                possibleRegions.Add([cell]);
            }
            else
            {
                var regions = possibleRegions.Where(r => r.Any(cell => sameCharNeighbors.Contains(cell))).ToList();

                switch (regions.Count)
                {
                    case 0:
                        possibleRegions.Add([cell]);
                        break;
                    case 1:
                        regions[0].Add(cell);
                        break;
                    default:
                    {
                        var merged = regions.SelectMany(r => r).ToList();
                        regions.ForEach(r => possibleRegions.Remove(r));
                        merged.Add(cell);
                        possibleRegions.Add(merged);
                        break;
                    }
                }
            }
        });

        return segmentedRegions.Values.SelectMany(v => v).ToList();
    }

    private int RegionPerimeter(CharGrid2D grid, Region region) =>
        region
            .Select(cell => new { cell, neighbors = cell.Neighbors(Direction2D.CardinalDirections, grid) })
            .Select(t => 4 - t.neighbors.Count(n => n.Char == t.cell.Char))
            .Sum();

    private int RegionSides(CharGrid2D grid, Region region)
    {
        // Each straight section of fence counts as a side
        var regionChar = region[0].Char;
        var perimeterCells = region
            .SelectMany(cell => Direction2D.CardinalDirections.Select(dir => (cell, dir)))
            .Select(t => (cell: t.cell,
                neighbor: grid.At(t.cell.X + t.dir.DeltaX, t.cell.Y + t.dir.DeltaY) ??
                          new Cell(t.cell.X + t.dir.DeltaX, t.cell.Y + t.dir.DeltaY, ' ')))
            .Where(t => t.neighbor.Char != regionChar)
            .ToList();

        var sides = 0;
        while (perimeterCells.Count > 0)
        {
            var reference = perimeterCells[0];
            perimeterCells.Remove(reference);
            
            var alignedOnX = reference.cell.X == reference.neighbor.X;
            var alignedOnY = reference.cell.Y == reference.neighbor.Y;
            if (new [] { alignedOnX, alignedOnY }.Count(b => b) != 1) Assert.Fail("Invalid state");
            
            var alignedCells = perimeterCells
                .Where(t => alignedOnX ? t.neighbor.Y == reference.neighbor.Y : t.neighbor.X == reference.neighbor.X)
                .Prepend(reference)
                .ToList();
            
            var distinct = alignedCells.Select(c => alignedOnX ? c.cell.X : c.cell.Y).Distinct().Count();
            
            perimeterCells.RemoveAll(alignedCells.Contains);

            sides += distinct;
        }

        return sides;
    }

    public override async Task<Answer> Part1(string input)
    {
        var grid = new CharGrid2D(input);
        var regions = Regions(grid);

        WriteIfTest($"Regions: {regions.Count}");

        var sum = 0;
        foreach (var region in regions)
        {
            var area = region.Count;
            var perimeter = RegionPerimeter(grid, region);
            var price = area * perimeter;

            WriteIfTest($"Region: {region[0].Char} Area: {area} Perimeter: {perimeter} = {price}");
            sum += price;
        }

        return sum;
    }

    public override async Task<Answer> Part2(string input)
    {
        var grid = new CharGrid2D(input);
        var regions = Regions(grid);

        WriteIfTest($"Regions: {regions.Count}");

        var sum = 0;
        foreach (var region in regions)
        {
            var area = region.Count;
            var sides = RegionSides(grid, region);
            var price = area * sides;

            WriteIfTest($"Region: {region[0].Char} Area: {area} Perimeter: {sides} = {price}");
            sum += price;
        }

        return sum;
    }
}