namespace AdventOfCode.Utils;

public static class Combinations
{
    public static IEnumerable<T[]> Generate<T>(T[] values, int length)
    {
        return GenerateCombination(Array.Empty<T>());

        IEnumerable<T[]> GenerateCombination(T[] current)
        {
            return current.Length == length 
                ? new[] { current } 
                : values.SelectMany(v => GenerateCombination(current.Append(v).ToArray()));
        }
    }
}