module AOC.Year2015.Day5

open AOC.Core.Puzzle
open AOC.Core.Input

let ``contains 3 vowels`` (s: string) =
    [ "a"; "e"; "i"; "o"; "u" ]
    |> List.map (fun v -> s |> splitEachChar |> Seq.where (fun c -> c = v) |> Seq.length)
    |> List.sum
    |> (fun l -> l >= 3)

let ``does not have forbidden combinations`` (s: string) =
    [ "ab"; "cd"; "pq"; "xy" ] |> List.map s.Contains |> List.contains true |> not

let ``contains same letter 2 in a row`` (s: string) =
    s
    |> splitEachChar
    |> (Seq.windowed 2)
    |> Seq.map List.ofSeq
    |> Seq.map (fun s ->
        match s with
        | [ a; b ] when a = b -> true
        | _ -> false)
    |> Seq.contains true

let ``contains a repeat letter with 1 between`` (s: string) =
    s
    |> splitEachChar
    |> (Seq.windowed 3)
    |> Seq.map List.ofSeq
    |> Seq.map (fun s ->
        match s with
        | [ a; b; c ] when a = c -> true
        | _ -> false)
    |> Seq.contains true


let ``contains same pair twice`` (s: string) =
    let pairs = s |> splitEachChar |> (Seq.windowed 2) |> Seq.indexed

    let mutable dict = Map.empty<string, int list>

    for (index, pair) in pairs do
        let sequence = String.concat "" <| pair

        dict <-
            dict
            |> (Map.change sequence
                <| (fun s ->
                    match s with
                    | None -> Some [ index ]
                    | Some value -> Some <| value @ [ index ]))

    Map.exists
        (fun k t ->
            match t with
            | [] -> false // No matches
            | [ a ] -> false // No duplicate
            | [ a; b ] when a + 1 = b -> false // Duplicates are from same sequence
            | _ -> true) // Either 2 where there is space, or 3+ items which is always good
        dict


let part1 (input: string) =
    let rules =
        [ ``contains 3 vowels``
          ``contains same letter 2 in a row``
          ``does not have forbidden combinations`` ]

    let applyRules (s: string) = List.map (fun r -> r s) rules

    let nice =
        input
        |> splitEachNewline
        |> Seq.map applyRules
        |> Seq.filter ((List.contains false) >> not)

    nice |> Seq.length

let part2 (input: string) =
    let rules =
        [ ``contains same pair twice``; ``contains a repeat letter with 1 between`` ]

    let applyRules (s: string) = List.map (fun r -> r s) rules

    let nice =
        input
        |> splitEachNewline
        |> Seq.map applyRules
        |> Seq.filter ((List.contains false) >> not)

    nice |> Seq.length

let day: Day = (part1 >> string, part2 >> string)
