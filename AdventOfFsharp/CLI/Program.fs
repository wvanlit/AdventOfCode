
open System.IO
open CLI
open Microsoft.Extensions.Configuration
open Spectre.Console

let config = ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                 .AddJsonFile("appsettings.development.json")
                 .Build()

let inputsPath = config["InputFolder"]

Console.intro config

let year, day = Console.choose config

let input = File.ReadAllText <| inputsPath + $"/{year}/{day}.txt"

let part1, part2 = Solution.solution[year][day]

AnsiConsole.MarkupLine($"[bold red]{year}[/]/[bold green]{day}[/]/[bold yellow]P1[/]")
AnsiConsole.WriteLine(part1 input)

AnsiConsole.MarkupLine($"[bold red]{year}[/]/[bold green]{day}[/]/[bold yellow]P2[/]")
AnsiConsole.WriteLine(part2 input)