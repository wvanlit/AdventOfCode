using AdventOfCode.Extensions;
using AdventOfCode.Utils;
using MoreLinq;

namespace AdventOfCode._2020;

[EventIdentifier(2020, 13)]
public class Day13 : Solution
{
    /**
     * Notes:
     * Bus leaves at 0
     * The bus with ID 5 departs from the sea port at timestamps 0, 5, 10, 15 (Modulo?)
     *
     * entries that show x must be out of service, so you decide to ignore them (Part 2?)
     *
     * P1: What is the ID of the earliest bus you can take to the airport
     *     multiplied by the number of minutes you'll need to wait for that bus?
     */
    public override Answer Part1(string input)
    {
        var i = input.SplitLines();
        var startingTime = int.Parse(i[0]);
        var busses = i[1].Split(",").Where(s => s != "x").Select(int.Parse).ToArray();

        var timeToWait = busses.Select(b => b - startingTime % b).ToList();

        var shortestTimeToWait = timeToWait.Min();
        var earliestBus = busses[timeToWait.FindIndex(t => t == shortestTimeToWait)];

        return earliestBus * shortestTimeToWait;
    }

    /**
     * Find t where t is the start of an order
     * 7,13 ,x,x,59 ,x,31 ,19
     * t,t+1,-,-,t+4,-,t+5,t+6
     *
     * surely the actual earliest timestamp will be larger than 100000000000000
     *
     *
     * P2: What is the earliest timestamp such that all of the listed bus IDs depart
     *     at offsets matching their positions in the list?
     */
    public override Answer Part2(string input)
    {
        var busses = input
            .SplitLines()[1]
            .Split(",")
            .Index()
            .Where(s => s.Value != "x")
            .Select(kv => (bus: int.Parse(kv.Value), offset: kv.Key))
            .ToArray();

        var remainders = new List<long>();
        var moduli = new List<long>();

        foreach (var (bus, i) in busses)
        {
            remainders.Add((bus - i));
            moduli.Add(bus);
        }

        return Modulo.ChineseRemainderTheorem(remainders.Zip(moduli).ToList());
    }
}