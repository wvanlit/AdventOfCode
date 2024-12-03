using System.Text.RegularExpressions;

namespace AdventOfCode._2024;

[EventIdentifier(2024, 3)]
public class Day3 : Solution
{
    public override Answer Part1(string input)
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

    public override Answer Part2(string input)
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