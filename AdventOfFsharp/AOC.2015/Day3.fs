module AOC.Year2015.Day3

open AOC.Core.Puzzle
open AOC.Core.Input

let parse input =
        input
        |> splitEachChar
        |> Seq.map (fun c ->
            match c with
            | "^" -> (0, 1)
            | "<" -> (-1, 0)
            | "v" -> (0, -1)
            | ">" -> (1, 0)
            | _ -> failwith "Invalid char!")

let part1 input =
    let directions = parse input

    let mutable pos = (0, 0)
    let mutable locations = Set [ pos ]
    for x, y in directions do
        pos <- ((fst pos) + x, (snd pos) + y)
        locations <- locations.Add(pos)
    
    locations |> Set.count

let part2 input =
    let directions = parse input

    let mutable santaPos = (0, 0)
    let mutable roboPos = (0, 0)
    
    let mutable locations = Set [ santaPos ]
    
    for i, (x, y) in directions |> Seq.indexed do
        if i % 2 = 0 then
            santaPos <- ((fst santaPos) + x, (snd santaPos) + y)
            locations <- locations.Add(santaPos)
        else
            roboPos <- ((fst roboPos) + x, (snd roboPos) + y)
            locations <- locations.Add(roboPos)
    
    locations |> Set.count

let day: Day = (part1 >> string, part2 >> string)
