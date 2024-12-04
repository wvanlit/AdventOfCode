namespace AoC.Shared.Extensions;

public static class StringExtensions
{
    public static int ToInt(this string s) => int.Parse(s);
    public static uint ToUint(this string s) => uint.Parse(s);
    public static ulong ToUlong(this string s) => ulong.Parse(s);

    public static string[] SplitLines(this string s) => 
        s.ReplaceLineEndings().Split(Environment.NewLine).ToArray();

    public static string[] SplitGroups(this string s) =>
        s.Split(Environment.NewLine + Environment.NewLine).ToArray();

    public static int[] ParseAsListOfInts(this string s) => s.SplitLines().Select(int.Parse).ToArray();
    
    public static int[] ParseAsListOfInts(this string s, string delim) => s.Trim().Split(delim).Select(int.Parse).ToArray();

    public static IEnumerable<string> RemoveEmptyStrings(this IEnumerable<string> strings) =>
        strings.Where(s => !string.IsNullOrWhiteSpace(s));
}