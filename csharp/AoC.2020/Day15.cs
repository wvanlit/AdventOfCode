using AoC.Shared;
using Xunit.Abstractions;

namespace AoC._2020;

public class Day15  (ITestOutputHelper outputHelper) : SolutionBase(2020, 15, false, outputHelper)
{
    /**
     * Notes:
     * Announce all the starting numbers.
     * If that was the first time the number has been spoken:
     * Say 0.
     * Otherwise, the number had been spoken before;
     * Announce how many turns apart the number is from when it was previously spoken.
     */
    public override async Task<Answer> Part1(string input)
    {
        var startingNumbers = input.Split(",").Select(uint.Parse).ToList();

        var currentTurn = 1u;
        var lastSpoken = new Dictionary<uint, uint>();
        var nextNumber = uint.MaxValue;

        foreach (var number in startingNumbers)
        {
            AnnounceTurn(number);
        }

        while (currentTurn < 2020)
        {
            AnnounceTurn(nextNumber);
        }

        return nextNumber;

        void AnnounceTurn(uint spoken)
        {
            if (lastSpoken.TryGetValue(spoken, out var previousTurn))
            {
                nextNumber = currentTurn - previousTurn;
            }
            else
            {
                // First time!
                nextNumber = 0u;
            }
            
            lastSpoken[spoken] = currentTurn;
            currentTurn += 1;
        }
    }

    public override async Task<Answer> Part2(string input)
    {
        var startingNumbers = input.Split(",").Select(uint.Parse).ToList();

        var currentTurn = 1u;
        var lastSpoken = new Dictionary<uint, uint>();
        var nextNumber = uint.MaxValue;

        foreach (var number in startingNumbers)
        {
            AnnounceTurn(number);
        }

        while (currentTurn < 30000000)
        {
            AnnounceTurn(nextNumber);
        }

        return nextNumber;

        void AnnounceTurn(uint spoken)
        {
            if (lastSpoken.TryGetValue(spoken, out var previousTurn))
            {
                nextNumber = currentTurn - previousTurn;
            }
            else
            {
                // First time!
                nextNumber = 0u;
            }
            
            lastSpoken[spoken] = currentTurn;
            currentTurn += 1;
        }
    }
}