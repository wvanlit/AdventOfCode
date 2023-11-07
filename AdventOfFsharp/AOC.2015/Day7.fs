module AOC.Year2015.Day7

open AOC.Core.Puzzle
open AOC.Core.Input
open FSharpx.Collections

type Input =
    | Wire of string
    | Signal of int

type Instruction =
    | Set of in1: Input * out: string
    | And of in1: Input * in2: Input * out: string
    | Or of in1: Input * in2: Input * out: string
    | LeftShift of in1: Input * in2: Input * out: string
    | RightShift of in1: Input * in2: Input * out: string
    | Not of int: Input * out: string

let parseInput (str: string) =
    match System.Int32.TryParse(str) with
    | true, int -> Signal int
    | _ -> Wire str

let parseInstruction line =
    match line |> splitEach " " |> Seq.toList with
    | [ signal; "->"; wire ] -> Set(parseInput signal, wire)
    | [ w1; "AND"; w2; "->"; w3 ] -> And(parseInput w1, parseInput w2, w3)
    | [ w1; "OR"; w2; "->"; w3 ] -> Or(parseInput w1, parseInput w2, w3)
    | [ w1; "LSHIFT"; w2; "->"; w3 ] -> LeftShift(parseInput w1, parseInput w2, w3)
    | [ w1; "RSHIFT"; w2; "->"; w3 ] -> RightShift(parseInput w1, parseInput w2, w3)
    | [ "NOT"; w1; "->"; w2 ] -> Not(parseInput w1, w2)
    | _ -> failwith $"Invalid input: {line}"

let part1 input =
    let instructions = input |> splitEachNewline |> Seq.map parseInstruction

    let mutable signalMap = Map.empty<string, int>
    let mutable queue = Queue.ofSeq instructions

    let getValue (i: Input) =
        match i with
        | Signal i -> Some i
        | Wire s when signalMap.ContainsKey s -> Some signalMap[s]
        | Wire s -> None

    while not queue.IsEmpty do
        let (instruction, q) = queue.Uncons

        let executed =
            match instruction with
            | Set(in1, out) ->
                match getValue in1 with
                | None -> false
                | Some value ->
                    signalMap <- signalMap.Add(out, value)
                    true
            | And(in1, in2, out) ->
                match (getValue in1), (getValue in2) with
                | Some value1, Some value2 ->
                    signalMap <- signalMap.Add(out, value1 &&& value2)
                    true
                | _ -> false
            | Or(in1, in2, out) ->
                match (getValue in1), (getValue in2) with
                | Some value1, Some value2 ->
                    signalMap <- signalMap.Add(out, value1 ||| value2)
                    true
                | _ -> false
            | LeftShift(in1, in2, out) ->
                match (getValue in1), (getValue in2) with
                | Some value1, Some value2 ->
                    signalMap <- signalMap.Add(out, value1 <<< value2)
                    true
                | _ -> false
            | RightShift(in1, in2, out) ->
                match (getValue in1), (getValue in2) with
                | Some value1, Some value2 ->
                    signalMap <- signalMap.Add(out, value1 >>> value2)
                    true
                | _ -> false
            | Not(in1, out) ->
                match getValue in1 with
                | None -> false
                | Some value ->
                    signalMap <- signalMap.Add(out, ~~~value)
                    true

        if executed then
            queue <- q
        else
            queue <- Queue.conj instruction q


    signalMap["a"]

let part2 input =
    let p1 = part1 input
    let initial = input
                  |> splitEachNewline
                  |> Seq.map parseInstruction
                  |> Seq.filter (fun i -> match i with
                                          | Set(in1, out) when out = "b" -> false
                                          | _ -> true)
                  |> Seq.toList
    
    
    
    let instructions = initial @ [ Set(Signal(p1), "b") ]

    let mutable signalMap = Map.empty<string, int>
    let mutable queue = Queue.ofSeq instructions

    let getValue (i: Input) =
        match i with
        | Signal i -> Some i
        | Wire s when signalMap.ContainsKey s -> Some signalMap[s]
        | Wire s -> None

    while not queue.IsEmpty do
        let (instruction, q) = queue.Uncons

        let executed =
            match instruction with
            | Set(in1, out) ->
                match getValue in1 with
                | None -> false
                | Some value ->
                    signalMap <- signalMap.Add(out, value)
                    true
            | And(in1, in2, out) ->
                match (getValue in1), (getValue in2) with
                | Some value1, Some value2 ->
                    signalMap <- signalMap.Add(out, value1 &&& value2)
                    true
                | _ -> false
            | Or(in1, in2, out) ->
                match (getValue in1), (getValue in2) with
                | Some value1, Some value2 ->
                    signalMap <- signalMap.Add(out, value1 ||| value2)
                    true
                | _ -> false
            | LeftShift(in1, in2, out) ->
                match (getValue in1), (getValue in2) with
                | Some value1, Some value2 ->
                    signalMap <- signalMap.Add(out, value1 <<< value2)
                    true
                | _ -> false
            | RightShift(in1, in2, out) ->
                match (getValue in1), (getValue in2) with
                | Some value1, Some value2 ->
                    signalMap <- signalMap.Add(out, value1 >>> value2)
                    true
                | _ -> false
            | Not(in1, out) ->
                match getValue in1 with
                | None -> false
                | Some value ->
                    signalMap <- signalMap.Add(out, ~~~value)
                    true

        if executed then
            queue <- q
        else
            queue <- Queue.conj instruction q

    signalMap["a"]


let day: Day = (part1 >> string, part2 >> string)
