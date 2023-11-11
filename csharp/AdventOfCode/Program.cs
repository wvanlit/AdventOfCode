using AdventOfCode;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

var year = int.Parse(config["Year"]!);
var day = int.Parse(config["Day"]!);

var inputFile = Path.Combine(config["Inputs"]!, config["Year"]!, config["day"] + ".txt");
var input = File.ReadAllText(inputFile).Trim();

AnsiConsole.MarkupLine($"\n[bold]AoC [green]{year}[/] day [blue]{day}[/][/]\n");

var solutionFactory = new SolutionFactory();
var solution = solutionFactory.Get(year, day);

AnsiConsole.WriteLine("Part 1");
try
{
    AnsiConsole.WriteLine(solution.Part1(input));
}
catch (Exception e)
{
    AnsiConsole.MarkupLine($"[red bold]{e.Message}[/]");
}

AnsiConsole.WriteLine("Part 2");
try
{
    AnsiConsole.WriteLine(solution.Part2(input));
}
catch (Exception e)
{
    AnsiConsole.MarkupLine($"[red bold]{e.Message}[/]");
}