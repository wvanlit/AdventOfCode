using System.Text.RegularExpressions;
using AdventOfCode.Extensions;

namespace AdventOfCode._2020;

[EventIdentifier(2020, 16)]
public class Day16 : Solution
{
    record Field(string Name, uint Range1Start, uint Range1End, uint Range2Start, uint Range2End)
    {
        public bool IsValid(uint value) =>
            (value >= Range1Start && value <= Range1End) ||
            (value >= Range2Start && value <= Range2End);
    };

    private static (Field[] fields, uint[] ticket, uint[][] tickets) Parse(string input)
    {
        var blocks = input.SplitGroups();

        var fields = blocks[0]
            .Split("\n")
            .Select(s => Regex.Match(s, @"([\w\s]+): (\d+)-(\d+) or (\d+)-(\d+)").Groups)
            .Select(m => new Field(m[1].Value, m[2].AsUint(), m[3].AsUint(), m[4].AsUint(), m[5].AsUint()))
            .ToArray();

        var ticket = blocks[1]
            .Replace("your ticket:\n", "")
            .Split(",")
            .Select(uint.Parse);

        var nearbyTickets = blocks[2]
            .Replace("nearby tickets:\n", "")
            .Split("\n")
            .Select(s => s.Split(",").Select(uint.Parse).ToArray());

        return (fields, ticket.ToArray(), nearbyTickets.ToArray());
    }


    public override Answer Part1(string input)
    {
        var (fields, _, nearbyTickets) = Parse(input);

        var invalidValues = new List<uint>();
        
        foreach (var ticket in nearbyTickets)
        {
            invalidValues.AddRange(ticket.Where(value => !fields.Any(f => f.IsValid(value))));
        }
        
        return invalidValues.Aggregate((p, c) => p + c);
    }

    public override Answer Part2(string input)
    {
        var (fields, myTicket, nearbyTickets) = Parse(input);
        var validTickets = nearbyTickets.Where(t => t.All(value => fields.Any(f => f.IsValid(value)))).ToArray();

        var possibleFieldDict = new Dictionary<int, Field[]>();
        
        for (var i = 0; i < myTicket.Length; i++)
        {
            var fieldIndex = i;
            possibleFieldDict[fieldIndex] = fields.Where(f => validTickets.All(t => f.IsValid(t[fieldIndex]))).ToArray();
        }

        var orderedByPossibilities = possibleFieldDict.ToArray().OrderBy(k => k.Value.Length);

        var selectedFields = new Dictionary<string, int>();

        foreach (var (idx, pf) in orderedByPossibilities)
        {
            var leftOverField = pf.Single(f => !selectedFields.ContainsKey(f.Name));
            selectedFields[leftOverField.Name] = idx;
        }

        // foreach (var kv in selectedFields)
        // {
        //     AnsiConsole.WriteLine($"{kv.Key,20} = {kv.Value:D2} = {myTicket[kv.Value]:D4}");
        // }

        return selectedFields
            .Where(kv => kv.Key.Contains("departure"))
            .Aggregate(1ul, (p, c) => p * myTicket[c.Value]);
    }
}