using System.Text.RegularExpressions;

namespace AdventOfCode.Extensions;

public static class RegexExtensions
{
    public static int AsInt(this Group group) => group.Value.ToInt();
    public static uint AsUint(this Group group) => group.Value.ToUint();

    public static Dictionary<string, string> NamedGroups(this Match match)
    {
        return match
            .Groups
            .Values
            .Where(group => !string.IsNullOrWhiteSpace(group.Name))
            .ToDictionary(group => group.Name, group => group.Value);
    }
}