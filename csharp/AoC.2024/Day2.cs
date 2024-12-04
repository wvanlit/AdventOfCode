using AoC.Shared;
using AoC.Shared.Extensions;
using MoreLinq;
using Spectre.Console;
using Xunit.Abstractions;

namespace AoC._2024;

public class Day2(ITestOutputHelper testOutputHelper) : SolutionBase(2024, 2, false, testOutputHelper)
{
    private static List<List<int>> ParseToSequences(string input) =>
        input
            .SplitLines()
            .Select(s => s.ParseAsListOfInts(" ").ToList())
            .ToList();

    private static bool IsDecreasing(IList<int> seq) => seq[0] - seq[1] > 0;
    private static bool IsIncreasing(IList<int> seq) => seq[0] - seq[1] < 0;
    private static bool IsSafe(IList<int> seq) => Math.Abs(seq[0] - seq[1]) <= 3 && Math.Abs(seq[0] - seq[1]) >= 1;

    private static bool SafeSequence(IEnumerable<IList<int>> seq) =>
        (seq.All(IsDecreasing) || seq.All(IsIncreasing)) && seq.All(IsSafe);

    public override async Task<Answer> Part1(string input)
    {
        return ParseToSequences(input)
            .Select(i => i.Window(2))
            .ToList()
            .Count(SafeSequence);
    }

    private static IEnumerable<IList<int>> FilterOutBadLevel(IEnumerable<IList<int>> sequences)
    {
        // Unwindow
        var unwindowed = sequences.Select(s => s[0]).Append(sequences.Last().Last()).ToList();
        
        AnsiConsole.WriteLine(unwindowed.ToDelimitedString(", "));

        for(var i = 0; i < unwindowed.Count(); i++)
        {
            var seq = unwindowed.Exclude(i, 1).Window(2).ToList();
            
            if (SafeSequence(seq))
            {
                AnsiConsole.WriteLine($"Removing {unwindowed[i]} makes it safe");
                return seq;
            }
        }
        
        return Enumerable.Empty<IList<int>>();
    }


    public override async Task<Answer> Part2(string input)
    {
        var sequences = ParseToSequences(input)
            .Select(i => i.Window(2))
            .ToList();
        
        var knownSafe = sequences.Where(SafeSequence).ToList();
        var unknownSafe = sequences.Where(seq => !SafeSequence(seq)).ToList();
        
        return knownSafe.Count + 
               unknownSafe.Select(FilterOutBadLevel).Where(s => s.Any()).Where(SafeSequence).Count();
        
    }
}