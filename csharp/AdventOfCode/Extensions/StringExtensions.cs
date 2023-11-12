namespace AdventOfCode.Extensions;

public static class StringExtensions
{
    public static int ToInt(this string s) => int.Parse(s);
    public static uint ToUint(this string s) => uint.Parse(s);
    public static ulong ToUlong(this string s) => ulong.Parse(s);
}