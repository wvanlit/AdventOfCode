using System.Reflection;

namespace AdventOfCode;

public class SolutionFactory
{
    private readonly List<(TypeInfo Type, int Year, int Day)> _solutions;

    public SolutionFactory()
    {
        _solutions = GetType().Assembly.DefinedTypes
            .Where(t => t.GetCustomAttributes(typeof(EventIdentifierAttribute), false).Any())
            .Select(t => new
            {
                Type = t,
                Attribute = (EventIdentifierAttribute)t.GetCustomAttribute(typeof(EventIdentifierAttribute), false)!
            })
            .Select(t => (t.Type, t.Attribute.Year, t.Attribute.Day))
            .ToList();
    }

    public Solution Get(int year, int day)
    {
        var solution = _solutions.Single((tuple => tuple.Year == year && tuple.Day == day)).Type;
        return (Solution)Activator.CreateInstance(solution.AsType())!;
    }
}