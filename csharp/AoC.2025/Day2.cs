using System.Text.RegularExpressions;
using AoC.Shared;
using Xunit;
using Xunit.Abstractions;

namespace AoC._2025;

public class Day2(ITestOutputHelper testOutputHelper) : SolutionBase(2025, 2, false, testOutputHelper)
{
    private List<(string, string)> Parse(string input) =>
        input.Split(",").Select(s => s.Split("-")).Select(s => (s.First(), s.Last())).ToList();

    private static long? IsInvalidPart1(string s)
    {
        // You can find the invalid IDs by looking for any ID which is made only of some sequence of digits repeated twice.
        // So, 55 (5 twice), 6464 (64 twice), and 123123 (123 twice) would all be invalid IDs.
        
        if (s.Length % 2 != 0) return null; // Repeats cannot be uneven
        var p1 = s[..(s.Length / 2)];
        var p2 = s[(s.Length / 2)..];
        
        return p1 == p2 ? long.Parse(s)  : null;
    }

    private static long? IsInvalidPart2(string s)
    {
        // An ID is invalid if it is made only of some sequence of digits repeated at least twice
        var maxPatternLength = s.Length / 2;
        var pattern = s[..maxPatternLength];

        for (int i = pattern.Length; i > 0; i--)
        {
            if (s.Length % i != 0) { continue; } // Skip if not divisible by pattern
            pattern = pattern.Substring(0, i);

            if (Regex.Matches(s, pattern).Count == s.Length / i)
            {
                return long.Parse(s);
            }
        }

        return null;
    }
    
    private List<long> InvalidIds(string first, string last, Func<string, long?> checker)
    {
        var list = new List<long>();

        for (var i = long.Parse(first); i <= long.Parse(last); i++)
        {
            var id = checker(i.ToString());
            if (id != null)
            {
                list.Add(i);
            }
        }
        
        WriteIfTest(() => $"{first}-{last} has invalid ids: {string.Join(", ", list)}");
        return list;
    }
    
    public override async Task<Answer> Part1(string input)
    {
        return Parse(input).SelectMany(i => InvalidIds(i.Item1, i.Item2, IsInvalidPart1)).Sum();
    }

    public override async Task<Answer> Part2(string input)
    {
        return Parse(input).SelectMany(i => InvalidIds(i.Item1, i.Item2, IsInvalidPart2)).Sum();
    }

    [Theory]
    [InlineData("11", 11)]
    [InlineData("22", 22)]
    [InlineData("1010", 1010)]
    [InlineData("1188511885", 1188511885)]
    [InlineData("446446",  446446)]
    public void IdentifiesInvalidIds(string input, long expected)
    {
        Assert.Equivalent(expected, IsInvalidPart1(input));
    }

    [Theory]
    [InlineData("101")]
    [InlineData("111")]
    [InlineData("1211")]
    [InlineData("38593862")]
    public void IdentifiesValidIds(string input)
    {
        Assert.Equivalent(null, IsInvalidPart1(input));
    }

    [Theory]
    [MemberData(nameof(FindsInvalidIdsForPart1Data))]
    public void FindsInvalidIdsForPart1(string first, string last, params long[] expected)
    {
        Assert.Equivalent(expected, InvalidIds(first, last, IsInvalidPart1));
    }
    
    public static TheoryData<string, string, long[]> FindsInvalidIdsForPart1Data() => new()
    {
        { "11", "22", [11, 22] },
        { "95", "115", [99]},
        { "998", "1012", [1010]},
        { "1188511880", "1188511890", [1188511885]},
        { "38593856", "38593862", [38593859]}
    };

    [Theory]
    [InlineData("12341234", 12341234)]
    [InlineData("123123123", 123123123)]
    [InlineData("1212121212", 1212121212)]
    [InlineData("1111111", 1111111)]
    [InlineData("11", 11)]
    [InlineData("22", 22)]
    [InlineData("99", 99)]
    [InlineData("1010", 1010)]
    [InlineData("1188511885", 1188511885)]
    [InlineData("222222", 222222)]
    [InlineData("446446", 446446)]
    [InlineData("38593859", 38593859)]
    [InlineData("565656", 565656)]
    [InlineData("824824824", 824824824)]
    [InlineData("2121212121", 2121212121)]
    public void IdentifiesInvalidIdsPart2(string input, long expected)
    {
        Assert.Equivalent(expected, IsInvalidPart2(input));
    }

    [Theory]
    [InlineData("101")]
    [InlineData("1112")]
    [InlineData("1211")]
    [InlineData("38593862")]
    [InlineData("1698522")]
    [InlineData("1698528")]
    [InlineData("222223")]
    [InlineData("565657")]
    [InlineData("824824825")]
    [InlineData("2121212118")]
    public void IdentifiesValidIdsPart2(string input)
    {
        Assert.Equivalent(null, IsInvalidPart2(input));
    }

    [Theory]
    [MemberData(nameof(FindsInvalidIdsForPart2Data))]
    public void FindsInvalidIdsForPart2(string first, string last, params long[] expected)
    {
        Assert.Equivalent(expected, InvalidIds(first, last, IsInvalidPart2));
    }

    public static TheoryData<string, string, long[]> FindsInvalidIdsForPart2Data() => new()
    {
        { "11", "22", [11, 22] },
        { "95", "115", [99, 111] },
        { "998", "1012", [999, 1010] },
        { "1188511880", "1188511890", [1188511885] },
        { "222220", "222224", [222222] },
        { "1698522", "1698528", [] },
        { "446443", "446449", [446446] },
        { "38593856", "38593862", [38593859] },
        { "565653", "565659", [565656] },
        { "824824821", "824824827", [824824824] },
        { "2121212118", "2121212124", [2121212121] }
    };
}