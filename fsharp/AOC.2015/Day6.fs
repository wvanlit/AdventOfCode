module AOC.Year2015.Day6

open System
open AOC.Core.Puzzle
open AOC.Core.Input

(*
    1 million lights
    1000 x 1000 grid
    x,y coords
    
    turn on, turn off, toggle
    
    [command] x1,y1 through x2,y2
    
    Goal: how many are on after instructions?
*)

type Coord = (int * int)

type Instruction =
    | TurnOn of Coord * Coord
    | TurnOff of Coord * Coord
    | Toggle of Coord * Coord

let parseCoord str =
    match str |> splitEach "," |> Seq.toList with
    | [ a; b ] -> (int a, int b)
    | _ -> failwith "Invalid coordinate"

let parseInstruction line =
    match line |> splitEach " " |> Seq.toList with
    | [ "turn"; "on"; c1; "through"; c2 ] -> TurnOn(parseCoord c1, parseCoord c2)
    | [ "turn"; "off"; c1; "through"; c2 ] -> TurnOff(parseCoord c1, parseCoord c2)
    | [ "toggle"; c1; "through"; c2 ] -> Toggle(parseCoord c1, parseCoord c2)
    | _ -> failwith $"invalid input: {line}"

let exec (grid: bool array2d) (i: Instruction) =
    let (c1, c2, f) =
        match i with
        | TurnOn(c1, c2) -> (c1, c2, (fun _ -> true))
        | TurnOff(c1, c2) -> (c1, c2, (fun _ -> false))
        | Toggle(c1, c2) -> (c1, c2, not)

    for x = fst c1 to fst c2 do
        for y = snd c1 to snd c2 do
            grid[x, y] <- f grid[x, y]

let execBrightness (grid: int array2d) (i: Instruction) =
    let (c1, c2, f) =
        match i with
        | TurnOn(c1, c2) -> (c1, c2, (fun i -> i + 1))
        | TurnOff(c1, c2) -> (c1, c2, (fun i -> Math.Max(i - 1, 0)))
        | Toggle(c1, c2) -> (c1, c2, (fun i -> i + 2))

    for x = fst c1 to fst c2 do
        for y = snd c1 to snd c2 do
            grid[x, y] <- f grid[x, y]

let countTrue (array2D: bool[,]) =
    let mutable count = 0

    Array2D.iter
        (fun item ->
            if item then
                count <- count + 1)
        array2D

    count

let countBrightness (array2D: int[,]) =
    let mutable count = 0
    Array2D.iter (fun item -> count <- count + item) array2D
    count

let part1 (input: string) =
    let grid = Array2D.create 1000 1000 false
    let instructions = input |> splitEachNewline |> Seq.map parseInstruction

    Seq.iter (exec grid) instructions

    countTrue grid

let part2 (input: string) =
    let grid = Array2D.create 1000 1000 0
    let instructions = input |> splitEachNewline |> Seq.map parseInstruction

    Seq.iter (execBrightness grid) instructions

    countBrightness grid

let day: Day = (part1 >> string, part2 >> string)
