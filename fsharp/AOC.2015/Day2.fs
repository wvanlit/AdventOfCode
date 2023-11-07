module AOC.Year2015.Day2

open System
open AOC.Core.Puzzle
open AOC.Core.Input

let parsePresents input =
    input
    |> splitEachNewline
    |> Seq.map (splitEach "x")
    |> Seq.map (Seq.map uint)
    |> Seq.map (fun p ->
        match (List.ofSeq p) with
        | [ l; w; h ] -> (l, w, h)
        | _ -> failwith "impossible state")



let part1 input =
    let wrap (l: uint, w: uint, h: uint) : uint =
        (2u * l * w + 2u * w * h + 2u * h * l) + Math.Min(l * w, Math.Min(w * h, h * l))

    parsePresents input |> Seq.map wrap |> Seq.sum

let part2 input =
    let cutRibbon (l: uint, w: uint, h: uint) : uint =
        let shortestSides = [ l; w; h ] |> List.sort |> List.take 2
        let doubleSides: uint = List.concat [ shortestSides; shortestSides ] |> List.sum
        let bow = l * w * h

        doubleSides + bow

    parsePresents input |> Seq.map cutRibbon |> Seq.sum


let day: Day = (part1 >> string, part2 >> string)
