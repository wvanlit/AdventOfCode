using System.Text.RegularExpressions;
using AoC.Shared;
using AoC.Shared.Extensions;
using Xunit;
using Xunit.Abstractions;
using static System.Math;

namespace AoC._2025;

public class Day3(ITestOutputHelper testOutputHelper) : SolutionBase(2025, 3, false, testOutputHelper)
{
    private List<List<int>> Parse(string input) =>
       input.SplitLines().Select(l => l.ToCharArray().Select(c => c - '0').ToList()).ToList();

    private long HighestJoltage(List<int> bank, int digits = 2)
    { 
        var max = bank[..^(digits-1)].Max();
       
        if(digits == 1) { return max; }

        var mi = bank.IndexOf(max);
        var prev = HighestJoltage(bank[(mi+1)..], digits - 1);
       
        return long.Parse($"{max}{prev}");
    }

    public override async Task<Answer> Part1(string input)
    {
        return Parse(input).Select(l => HighestJoltage(l, digits: 2)).Sum();
    }

    public override async Task<Answer> Part2(string input)
    {
        return Parse(input).Select(l => HighestJoltage(l, digits: 12)).Sum();
    }   
    
    [Theory]
    [InlineData("987654321111111", 98)]
    [InlineData("811111111111119", 89)]
    [InlineData("818181911112111", 92)]
    public void VerifyHighestJoltage_2_digit(string input, long expected)
    {
        var bank = Parse(input)[0];
        var result = HighestJoltage(bank, 2);
        
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData("987654321111111", 987654321111)]
    [InlineData("811111111111119", 811111111119)]
    [InlineData("234234234234278", 434234234278)]
    [InlineData("818181911112111", 888911112111)]
    public void VerifyHighestJoltage_12_digit(string input, long expected)
    {
        var bank = Parse(input)[0];
        var result = HighestJoltage(bank, 12);
        
        Assert.Equal(expected, result);
    }
}