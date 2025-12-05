using AoC.Shared;
using AoC.Shared.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace AoC._2025;

public class Day5(ITestOutputHelper testOutputHelper) : SolutionBase(2025, 5, false, testOutputHelper)
{
    private static (List<(long From, long To)> Fresh, List<long> Ids) Parse(string input)
    {
        var groups = input.SplitGroups();
        var fresh = groups[0]
            .SplitLines()
            .Select(s => s.Split("-"))
            .Select(s => (long.Parse(s[0]), long.Parse(s[1])))
            .ToList(); 
        var ids = groups[1].SplitLines().Select(long.Parse).ToList();
        
        return (fresh, ids);
    }

    private static bool IsFresh(long id, List<(long From, long To)> fresh)
    {
        return fresh.Any(f => f.To >= id && f.From <= id);
    }
    
    private static List<(long From, long To)> MergeRanges(List<(long From, long To)> ranges)
    {
        var sorted = ranges.OrderBy(r => r.From).ThenBy(r => r.To).ToList();
        List<(long From, long To)> merged;
        long previousCount;
        
        do
        {
            previousCount = sorted.Count;
            merged = [];
            var current = sorted[0];
            
            foreach (var range in sorted.Skip(1))
            {
                if (current.To >= range.From - 1) 
                {
                    current = (current.From, Math.Max(current.To, range.To));
                }
                else
                {
                    merged.Add(current);
                    current = range;
                }
            }
            
            merged.Add(current);
            sorted = merged;
        } while (sorted.Count < previousCount);
        
        return merged;
    }
    

    private static long SumTotalIdsInRanges(List<(long From, long To)> ranges)
    {
        return ranges.Select(r => r.To - r.From + 1).Sum();
    }

    public override async Task<Answer> Part1(string input)
    {
        var (fresh, ids) = Parse(input);
        
        return ids.Count(id => IsFresh(id, fresh));
    }

    public override async Task<Answer> Part2(string input)
    {
        var (fresh, _) = Parse(input);
        
        var merged = MergeRanges(fresh);
        
        return SumTotalIdsInRanges(merged);
    }   
    
    
    [Fact]
    public void MergesRangesCorrectly()
    {
        var ranges = new List<(long From, long To)>
        {
            (1, 5),
            (2, 6),
            (8, 10),
            (9, 12),
            (15, 20)
        };
        
        var merged = MergeRanges(ranges);
        
        Assert.Equal(3, merged.Count);
        Assert.Contains((1, 6), merged);
        Assert.Contains((8, 12), merged);
        Assert.Contains((15, 20), merged);
        
        var total = SumTotalIdsInRanges(merged);
        Assert.Equal(6 + 5 + 6, total);
    }
    
    [Fact]
    public void MergesHighRangesCorrectly()
    {
        var merged = MergeRanges([
            (112803099155787,116430385712237),
            (112803099155787,119981015088069)
        ]);
        
        Assert.Single(merged);
        Assert.Contains((112803099155787, 119981015088069), merged);
    }
    
    [Fact]
    public void MergesUnsortedRangesCorrectly()
    {
        var ranges = new List<(long From, long To)>
        {
            (15, 20),
            (1, 5),
            (9, 12),
            (2, 6),
            (8, 10)
        };
        
        var merged = MergeRanges(ranges);
        
        Assert.Equal(3, merged.Count);
        Assert.Contains((1, 6), merged);
        Assert.Contains((8, 12), merged);
        Assert.Contains((15, 20), merged);
        
        var total = SumTotalIdsInRanges(merged);
        Assert.Equal(6 + 5 + 6, total);
    }
}