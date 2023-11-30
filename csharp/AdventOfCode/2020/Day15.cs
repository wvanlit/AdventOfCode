namespace AdventOfCode._2020;

[EventIdentifier(2020, 15)]
public class Day15 : Solution
{
    /**
     * Notes:
     * Announce all the starting numbers.
     * If that was the first time the number has been spoken:
     * Say 0.
     * Otherwise, the number had been spoken before;
     * Announce how many turns apart the number is from when it was previously spoken.
     */
    public override Answer Part1(string input)
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

    public override Answer Part2(string input)
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