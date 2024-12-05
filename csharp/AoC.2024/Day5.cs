using AoC.Shared;
using AoC.Shared.Extensions;
using Snapshooter.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace AoC._2024;

using RuleDict = Dictionary<int, int[]>;

public class Day5(ITestOutputHelper testOutputHelper) : SolutionBase(2024, 5, false, testOutputHelper)
{
    private bool PageInCorrectOrder(int[] page, RuleDict forwardRules, RuleDict backwardRules)
    {
        for (var i = 0; i < page.Length; i++)
        {
            var item = page[i];

            var remaining = page.Skip(i + 1).ToArray();

            if (forwardRules.TryGetValue(item, out int[]? forwardRuleNumbers))
            {
                forwardRuleNumbers = forwardRuleNumbers.Where(page.Contains).ToArray();

                var ruleAllInRemaining = forwardRuleNumbers.All(r => remaining.Contains(r));

                if (!ruleAllInRemaining)
                {
                    return false;
                }
            }

            var passed = page.Take(i).ToArray();

            if (backwardRules.TryGetValue(item, out int[]? backwardRuleNumbers))
            {
                backwardRuleNumbers = backwardRuleNumbers.Where(page.Contains).ToArray();

                var ruleAllInPassed = backwardRuleNumbers.All(r => passed.Contains(r));

                if (!ruleAllInPassed)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public (RuleDict forwardRules, RuleDict backwardRules, int[][] pages) Parse(string input)
    {
        var g = Input.NormalizeLineEndings().Split("\n\n").ToArray();

        Assert.Equal(2, g.Length);

        var rules = g[0].SplitLines().Select(s => s.ParseAsListOfInts("|")).ToArray();

        var forwardRules = rules
            .GroupBy(t => t[0])
            .ToDictionary(g => g.Key, g => g.Select(t => t[1]).ToArray());

        var backwardRules = rules
            .GroupBy(t => t[1])
            .ToDictionary(g => g.Key, g => g.Select(t => t[0]).ToArray());

        var pages = g[1].SplitLines().Select(s => s.ParseAsListOfInts(",")).ToArray();

        return (forwardRules, backwardRules, pages);
    }

    public int FindMiddleNumber(int[] page)
    {
        return page.Length % 2 == 0 ? page.Length / 2 - 1 : page.Length / 2;
    }
    
    public override async Task<Answer> Part1(string input)
    {
        var (forwardRules, backwardRules, pages) = Parse(input);

        var sum = 0;
        foreach (var page in pages)
        {
            if (PageInCorrectOrder(page, forwardRules, backwardRules))
            {
                sum += page[FindMiddleNumber(page)];
            }
        }

        return sum;
    }

    public override async Task<Answer> Part2(string input)
    {
        var (forwardRules, backwardRules, pages) = Parse(input);
        
        var incorrectPages = pages.Where(p => !PageInCorrectOrder(p, forwardRules, backwardRules)).ToArray();

        var sum = 0;
        var ruleComparer = new RuleComparer(forwardRules, backwardRules);
        foreach (var page in incorrectPages)
        {
            var correct = page.Order(ruleComparer).ToArray();
            if (!PageInCorrectOrder(correct, forwardRules, backwardRules))
            {
                throw new Exception("Incorrect page");
            }
            
            sum += correct[FindMiddleNumber(correct)];
        }
        
        return sum;
    }

    class RuleComparer(RuleDict forwardRules, RuleDict backwardRules) : IComparer<int>
    {
        const int XisBeforeY = -1;
        const int XisAfterY = 1;
        const int XandYAreEqual = 0;
        
        public int Compare(int x, int y)
        {
            if (forwardRules.TryGetValue(x, out int[]? forwardRuleNumbers))
            {
                if (forwardRuleNumbers.Contains(y))
                {
                    return XisBeforeY;
                }
            }

            if (backwardRules.TryGetValue(x, out int[]? backwardRuleNumbers))
            {
                if (backwardRuleNumbers.Contains(y))
                {
                    return XisAfterY;
                }
            }

            return XandYAreEqual;
        }
    }
}