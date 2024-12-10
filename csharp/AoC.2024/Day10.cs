using AoC.Shared;
using AoC.Shared.DataStructures;
using AoC.Shared.Extensions;
using Xunit;
using Xunit.Abstractions;
using static AoC.Shared.DataStructures.CharGrid2D;

namespace AoC._2024;

public class Day10(ITestOutputHelper outputHelper) :
    SolutionBase(year: 2024, day: 10, runTest: false, outputHelper)
{
    private static bool CanReach(Graph<Cell> graph, GraphNode<Cell> start, GraphNode<Cell> end)
    {
        var path = graph.ShortestPathBfs(start, end, skipCondition: cell => cell.Value.Char is '0' or '9');
        if (path == null) return false;
        if (path.Count != 10) return false;
        if (path.Count(c => c.Value.Char is '9' or '0') != 2) throw new Exception("Invalid path");

        return true;
    }

    private (Graph<Cell> graph, List<GraphNode<Cell>> trailHeads, List<GraphNode<Cell>> peaks) Parse(string input)
    {
        var grid = new CharGrid2D(input);
        var graph = Graph<Cell>.From(grid, char.IsAsciiDigit);

        graph.PruneNeighbors(
            predicate: (node, neighbor) => (Math.Abs(node.Value.Char - neighbor.Value.Char) > 1)
            //onPrune: (node, neighbor) => WriteIfTest($"Pruned: {node.Value} -> {neighbor.Value}"
        );

        var trailHeads = graph.Nodes.Where(n => n.Value.Char == '0').ToList();
        OutputHelper.WriteLine($"Trail Heads ({trailHeads.Count})");

        var peaks = graph.Nodes.Where(n => n.Value.Char == '9').ToList();
        OutputHelper.WriteLine($"Peaks ({peaks.Count})");

        return (graph, trailHeads, peaks);
    }

    public override async Task<Answer> Part1(string input)
    {
        var (graph, trailHeads, peaks) = Parse(input);
        var sum = 0;

        foreach (var trailHead in trailHeads)
        {
            var trailCount = 0;

            Parallel.ForEach(peaks, peak =>
            {
                if (CanReach(graph, trailHead, peak)) Interlocked.Increment(ref trailCount);
            });

            sum += trailCount;
        }

        OutputHelper.WriteLine($"Sum: {sum}");

        return sum;
    }

    private int DistinctPaths(Graph<Cell> graph, GraphNode<Cell> start, GraphNode<Cell> end)
    {
        var paths = graph.GetAllPathsDfs(start, end,
                nodeSkipCondition: cell => cell.Value.Char is '0' or '9',
                pathSkipCondition: path => path.Count > 12).Where(p => p.Count == 10).ToList();

        if (paths.Count == 0) return 0;
        
        OutputHelper.WriteLine($"Paths from {start.Value} to {end.Value}: {paths.Count}");
        return paths.Count;
    }

    public override async Task<Answer> Part2(string input)
    {
        var (graph, trailHeads, peaks) = Parse(input);
        var sum = 0;

        foreach (var trailHead in trailHeads)
        {
            var trailCount = 0;

            Parallel.ForEach(peaks,
                peak => { Interlocked.Add(ref trailCount, DistinctPaths(graph, trailHead, peak)); });

            sum += trailCount;
        }

        OutputHelper.WriteLine($"Sum: {sum}");

        return sum;
    }

    [Fact]
    public async Task SimplifiedPart1()
    {
        var input = """
                    ..202..
                    ...1...
                    ...2...
                    6543456
                    7.....7
                    8.....8
                    9.....9
                    """;

        var answer = await Part1(input);

        Assert.Equal("2", answer.Value);
    }

    [Fact]
    public async Task MoreComplexPart1()
    {
        var input = """
                    ..90..9
                    ...1.98
                    ...2..7
                    6543456
                    765.987
                    876....
                    987....
                    """;

        var answer = await Part1(input);

        Assert.Equal("4", answer.Value);
    }

    [Fact]
    public async Task NoWrapAroundAllowed()
    {
        var input = "4321098765\n4321098765";

        var answer = await Part1(input);

        Assert.Equal("0", answer.Value);
    }
}