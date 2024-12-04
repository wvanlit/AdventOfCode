using System.Text.RegularExpressions;
using AoC.Shared;
using Xunit.Abstractions;

namespace AoC._2024;

public class Day3(ITestOutputHelper testOutputHelper) : SolutionBase(2024, 3, false, testOutputHelper)
{
    public override async Task<Answer> Part1(string input)
    {
        var sum = 0;
        foreach (Match m in Regex.Matches(input, "mul\\((\\d+),(\\d+)\\)", RegexOptions.Multiline))
        {
            var a = int.Parse(m.Groups[1].Value);
            var b = int.Parse(m.Groups[2].Value);

            sum += a * b;
        }

        return sum;
    }

    public override async Task<Answer> Part2(string input)
    {
        var sum = 0;
        var enabled = true;
        foreach (Match m in Regex.Matches(input, @"(mul\((\d+),(\d+)\))|don't\(\)|do\(\)", RegexOptions.Multiline))
        {
            if (m.Value == "don't()")
            {
                enabled = false;
                continue;
            }

            if (m.Value == "do()")
            {
                enabled = true;
                continue;
            }

            var a = int.Parse(m.Groups[2].Value);
            var b = int.Parse(m.Groups[3].Value);

            if (!enabled) continue;
            sum += a * b;
        }

        return sum;
    }
}