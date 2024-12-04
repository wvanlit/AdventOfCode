using System.Text.RegularExpressions;
using AoC.Shared;
using AoC.Shared.Extensions;
using MoreLinq.Extensions;
using Xunit.Abstractions;

namespace AoC._2020;

public class Day19  (ITestOutputHelper outputHelper) : SolutionBase(2020, 19, false, outputHelper)
{
    record Rule
    {
        private string? Literal;
        private List<int>? Order;
        private List<int>? Choice1;
        private List<int>? Choice2;

        private string? Pattern = null;

        public static Rule FromPattern(string pattern) => new Rule("") { Pattern = pattern };

        public Rule(string line)
        {
            var match = Regex.Match(line,
                """(?<order>^(?:\d+ ?)+$)|(?<literal>^"\w+"$)|(?<choice>^(?:\d+ ?)+\| (?:\d+ ?)+$)""");
            var groups = match.NamedGroups();

            if (groups.TryGetValue("order", out var order) && !string.IsNullOrWhiteSpace(order))
            {
                Order = order.Split(" ").Select(int.Parse).ToList();
            }
            else if (groups.TryGetValue("literal", out var literal) && !string.IsNullOrWhiteSpace(literal))
            {
                Literal = literal.Replace("\"", "");
            }
            else if (groups.TryGetValue("choice", out var choice) && !string.IsNullOrWhiteSpace(choice))
            {
                var parts = choice.Split("|").ToArray();
                Choice1 = parts[0].Trim().Split(" ").Select(int.Parse).ToList();
                Choice2 = parts[1].Trim().Split(" ").Select(int.Parse).ToList();
            }
        }

        public override string ToString()
        {
            if (Literal is not null)
                return Literal;
            if (Order is not null)
                return Order.ToDelimitedString(",");
            if (Choice1 is not null && Choice2 is not null)
                return $"{Choice1.ToDelimitedString(",")} | {Choice2.ToDelimitedString(",")}";
            throw new Exception("Invalid state!");
        }

        public string GeneratePattern(Dictionary<int, Rule> rules)
        {
            Pattern ??= CreatePattern(rules);
            return Pattern;
        }

        private string CreatePattern(Dictionary<int, Rule> rules)
        {
            if (Literal is not null)
                return Literal;

            if (Order is not null)
                return Order.Select(r => rules[r].GeneratePattern(rules)).ToDelimitedString("");

            if (Choice1 is not null && Choice2 is not null)
            {
                var c1 = Choice1.Select(r => rules[r].GeneratePattern(rules)).ToDelimitedString("");
                var c2 = Choice2.Select(r => rules[r].GeneratePattern(rules)).ToDelimitedString("");
                return $"(?:(?:{c1})|(?:{c2}))";
            }

            throw new Exception("Invalid state!");
        }
    }

    private (int index, Rule rule) Parse(string line)
    {
        var parts = line.Split(":").ToArray();
        return (parts[0].ToInt(), new Rule(parts[1].Trim()));
    }

    public override async Task<Answer> Part1(string input)
    {
        var parts = input.SplitGroups();

        var rules = parts[0]
            .Trim()
            .Split("\n")
            .Select(Parse)
            .OrderBy(x => x.index)
            .ToDictionary(
                keySelector: tuple => tuple.index,
                elementSelector: tuple => tuple.rule);

        var inputs = parts[1].SplitLines().ToArray();

        var pattern = $"^{rules[0].GeneratePattern(rules)}$";

        return inputs.Count(s => Regex.IsMatch(s, pattern));
    }

    public override async Task<Answer> Part2(string input)
    {
        input = input
            .Replace("8: 42", "8: \"TODO\"")
            .Replace("11: 42 31", "11: \"TODO\"");

        var parts = input.SplitGroups();

        var rules = parts[0]
            .Trim()
            .Split("\n")
            .Select(Parse)
            .OrderBy(x => x.index)
            .ToDictionary(
                keySelector: tuple => tuple.index,
                elementSelector: tuple => tuple.rule);

        // Too lazy to generate these, so just hard code them
        var r42 = rules[42].GeneratePattern(rules);
        var r31 = rules[31].GeneratePattern(rules);

        rules[8] = Rule.FromPattern($"({r42})+");

        
        // Regex with balancing group definition
        // https://learn.microsoft.com/en-us/dotnet/standard/base-types/grouping-constructs-in-regular-expressions?redirectedfrom=MSDN#balancing-group-definitions
        rules[11] = Rule.FromPattern(
            // Matches one or more of r42. Each match is pushed onto the '42' stack
            $"(?<42>({r42}))+" +
            // Matches one or more of r31. Each match pops from the '42' stack
            $"(?<-42>({r31}))+" +
            //A conditional that checks if the '42' stack is empty. If not, it fails the match
            $"(?(42)(?!))");

        var inputs = parts[1].SplitLines().ToArray();

        var pattern = $"^{rules[0].GeneratePattern(rules)}$";

        return inputs.Count(s => Regex.IsMatch(s, pattern));
    }
}