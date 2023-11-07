module CLI.Console

open Microsoft.Extensions.Configuration
open Microsoft.FSharp.Core
open Spectre.Console

let intro (config: IConfiguration) =
    AnsiConsole.Clear()

    let title = FigletText(FigletFont.Load("starwars.flf"), "aoc")
    title.Color <- Color.Green

    AnsiConsole.Write(title)

    AnsiConsole.MarkupLine("[bold blue]Inputs at:[/] [italic]" + config["InputFolder"] + "[/]")
    
    
let choose (config: IConfiguration): uint * uint =
    let year = config["ForceYear"]
    let day = config["ForceDay"]
    
    if year <> null && day <> null then
        (year |> uint, day |> uint)
    else
        failwith "TODO: day selection"