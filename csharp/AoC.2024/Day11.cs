using AoC.Shared;
using AoC.Shared.Extensions;
using Xunit.Abstractions;

namespace AoC._2024;

public class Day11(ITestOutputHelper outputHelper) :
    SolutionBase(year: 2024, day: 11, runTest: false, outputHelper)
{
    public override async Task<Answer> Part1(string input)
    {
        var stones = input.ParseAsListOfInts(" ").Select(i => (long)i).ToArray();

        for (var i = 0; i < 25; i++) // Blink 25 times
        {
            stones = stones.SelectMany(Blink).ToArray();
        }

        return stones.Length;
    }

    public override async Task<Answer> Part2(string input)
    {
        var stones = input.ParseAsListOfInts(" ").Select(i => (long)i).ToDictionary(k => k, _ => 1L);
        var sum = (Dictionary<long, long> s) => s.Values.Sum();

        for (var i = 0; i < 75; i++) // Blink 75 times
        {
            var temp = new Dictionary<long, long>();
            foreach (var (stone, count) in stones)
            {
                var newStones = Blink(stone);
                foreach (var newStone in newStones)
                {
                    temp[newStone] = temp.GetValueOrDefault(newStone) + count;
                }
            }
            stones = temp;
            
            OutputHelper.WriteLine($"{i + 1}: {sum(stones)}");
        }

        return sum(stones);
    }
    
    /// <summary>
    /// If the stone is engraved with the number 0, it is replaced by a stone engraved with the number 1.
    /// If the stone is engraved with a number that has an even number of digits, it is replaced by two stones.
    /// The left half of the digits are engraved on the new left stone, and the right half of the digits are engraved on the new right stone.
    /// (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.)
    /// If none of the other rules apply, the stone is replaced by a new stone; the old stone's number multiplied by 2024 is engraved on the new stone.
    /// </summary>
    public long[] Blink(long stone)
    {
        return stone switch {
            0 => [1],
            _ when stone.ToString().Length % 2 == 0 => CutStoneInHalf(stone),
            _ => [stone * 2024]
        };
        
        long[] CutStoneInHalf(long s)
        {
            var half = s.ToString().Length / 2; // Will always be even
            var left = long.Parse(s.ToString().Substring(0, half));
            var right = long.Parse(s.ToString().Substring(half));
            return [left, right];
        }
    }
}