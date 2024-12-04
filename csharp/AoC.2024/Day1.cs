using AoC.Shared;
using AoC.Shared.Extensions;
using Xunit.Abstractions;

namespace AoC._2024;

public class Day1(ITestOutputHelper testOutputHelper) : SolutionBase(2024, 1, false, testOutputHelper)
{
    private (List<int> left, List<int> right) CreateLists(string input)
    {
        var lines = input
            .SplitLines()
            .Select(s => s.Split("   "))
            .ToList();
        
        var leftList = lines.Select(s => int.Parse(s[0])).Order();
        var rightList = lines.Select(s => int.Parse(s[1])).Order();

        return (leftList.ToList(), rightList.ToList());
    }
    
    public override async Task<Answer> Part1(string input)
    {
        var (leftList, rightList) = CreateLists(input);   
        var pairs = leftList.Zip(rightList);
        
        return pairs.Select(p => Math.Abs(p.First - p.Second)).Sum();
    }

    public override async Task<Answer> Part2(string input)
    {
        var (leftList, rightList) = CreateLists(input);
        
        return leftList.Select(i => i * rightList.Count(n => n == i)).Sum();
    }
}