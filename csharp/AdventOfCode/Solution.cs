namespace AdventOfCode;

[AttributeUsage(AttributeTargets.Class)]
public class EventIdentifierAttribute : Attribute
{
    public readonly int Year;
    public readonly int Day;

    public EventIdentifierAttribute(int year, int day)
    {
        Year = year;
        Day = day;
    }
}

public abstract class Solution
{
    public abstract Answer Part1(string input);

    public abstract Answer Part2(string input);
}

public record Answer(string Value)
{
    public static implicit operator Answer(string value) => new(value);
    public static implicit operator Answer(int value) => new(value.ToString());
    public static implicit operator Answer(long value) => new(value.ToString());
    public static implicit operator Answer(ulong value) => new(value.ToString());
    public static implicit operator Answer(uint value) => new(value.ToString());

    public static implicit operator string(Answer answer) => answer.Value;
}