using System.Text.RegularExpressions;
using AoC.Shared;
using AoC.Shared.DataStructures;
using AoC.Shared.Extensions;
using AoC.Shared.Utils;
using Xunit;
using Xunit.Abstractions;
using static System.Math;

namespace AoC._2025;

public class Day5(ITestOutputHelper testOutputHelper) : SolutionBase(2025, 5, true, testOutputHelper)
{
    private (List<(long, long)> Fresh, List<long> Ids) Parse(string input)
    {
        var groups = input.SplitGroups();
        var fresh = groups[0].SplitLines().Select(s => s.Split("-")).Select(s => (long.Parse(s[0]), long.Parse(s[1]))); //TODO : order from low to high
        var ids = groups[1].SplitLines().Select(long.Parse);
        return (fresh, ids);
    }

    private bool IsFresh(long id, List<(long From, long To)> fresh)
    {
        return fresh.Any(f => f.To <= id && f.From >= id);
    }

    public override async Task<Answer> Part1(string input)
    {
        var (fresh, ids) = Parse(input);
        
        return ids.Count(id => IsFresh(id, fresh));
    }

    public override async Task<Answer> Part2(string input)
    {
        return Answer.Failed;
    }   
}