module AOC.Year2015.Day1

open AOC.Core.Puzzle
open AOC.Core.Input

let parse (input: string) = input |> splitEachChar |> Seq.map (fun c -> if c = "(" then 1 else -1)

let part1 input = parse input |> Seq.sum

let part2 input =
    parse input
    |> Seq.indexed
    |> Seq.reduce (fun (fo, acc) (fc, dir) -> if acc <> -1 then (fc, acc + dir) else (fo, acc))
    |> fst
    |> fun x -> x + 1

let day: Day = (part1 >> string, part2 >> string)
