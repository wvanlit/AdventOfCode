using System.Text;
using AoC.Shared;
using AoC.Shared.Extensions;
using MoreLinq;
using Xunit;
using Xunit.Abstractions;

namespace AoC._2025;

public class Day6(ITestOutputHelper testOutputHelper) : SolutionBase(
    year: 2025,
    day: 6,
    runTest: false,
    outputHelper: testOutputHelper)
{
    private static List<(List<long> Numbers, string Operator)> ParseColumns(string input)
    {
        var lines = input.SplitLines();
        var operators = lines[^1].Split(" ").RemoveEmptyStrings().ToArray();
        var numbers = new List<List<long>>();

        // List per row
        foreach (var line in lines[..^1])
        {
            numbers.Add(line.Split(" ").RemoveEmptyStrings().Select(long.Parse).ToList());
        }

        var problems = new List<(List<long> Numbers, string Operator)>();

        // Transpose to List per column
        for (var problemIndex = 0; problemIndex < operators.Length; problemIndex++)
        {
            var @operator = operators[problemIndex];
            var nums = numbers.Select(line => line[problemIndex]).ToList();

            problems.Add((nums, @operator));
        }

        return problems;
    }

    private static List<(List<long> Numbers, string Operator)> ParseColumnsRightToLeft(string input)
    {
        var lines = input.SplitLines();
        var width = lines.Max(l => l.Length);

        // Pad so every row has the same length
        var paddedLines = lines.Select(l => l.PadRight(width)).ToArray();
        var operatorRow = paddedLines[^1];
        var numberRows = paddedLines[..^1];

        var problems = new List<(List<long> Numbers, string Operator)>();
        var currentNumbers = new List<long>();
        string? currentOperator = null;

        bool IsSeparatorColumn(int column) => paddedLines.All(row => row[column] == ' ');

        for (var column = 0; column < width; column++)
        {
            if (IsSeparatorColumn(column))
            {
                if (currentNumbers.Count > 0)
                {
                    problems.Add((currentNumbers, currentOperator!));
                    currentNumbers = [];
                    currentOperator = null;
                }

                continue;
            }

            if (currentNumbers.Count == 0)
            {
                currentOperator = operatorRow[column].ToString();
            }

            var digits = numberRows.Select(row => row[column]).Where(char.IsDigit).ToArray();

            currentNumbers.Add(long.Parse(new string(digits)));
        }

        problems.Add((currentNumbers, currentOperator!));

        return problems;
    }

    private static Func<long, long, long> Operator(string op)
    {
        return op switch
        {
            "+" => (a, b) => a + b,
            "*" => (a, b) => a * b,
            _ => throw new ArgumentOutOfRangeException(nameof(op), op, null)
        };
    }


    public override async Task<Answer> Part1(string input)
    {
        var problems = ParseColumns(input);
        var answers = problems.Select(p => p.Numbers.Aggregate(Operator(p.Operator))).ToList();
        return answers.Sum();
    }

    public override async Task<Answer> Part2(string input)
    {
        var problems = ParseColumnsRightToLeft(input);
        var answers = problems.Select(p => p.Numbers.Aggregate(Operator(p.Operator))).ToList();
        return answers.Sum();
    }

    [Fact]
    public void ParseColumnsRightToLeft_SampleMatchesExpectedColumns()
    {
        var problems = ParseColumnsRightToLeft(SampleRightToLeftInput);

        Assert.Collection(problems,
            first =>
            {
                Assert.Equal("*", first.Operator);
                Assert.Equal(new long[] { 1, 24, 356 }, first.Numbers);
            },
            second =>
            {
                Assert.Equal("+", second.Operator);
                Assert.Equal(new long[] { 369, 248, 8 }, second.Numbers);
            },
            third =>
            {
                Assert.Equal("*", third.Operator);
                Assert.Equal(new long[] { 32, 581, 175 }, third.Numbers);
            },
            fourth =>
            {
                Assert.Equal("+", fourth.Operator);
                Assert.Equal(new long[] { 623, 431, 4 }, fourth.Numbers);
            });
    }

    [Fact]
    public async Task Part2_SampleInput_MatchesExpectedAnswer()
    {
        var answer = await Part2(SampleRightToLeftInput);
        Assert.Equal(3263827, answer);
    }

    private static readonly string SampleRightToLeftInput = string.Join("\n", new[]
    {
        "123 328  51 64 ",
        " 45 64  387 23 ",
        "  6 98  215 314",
        "*   +   *   +  "
    });
}