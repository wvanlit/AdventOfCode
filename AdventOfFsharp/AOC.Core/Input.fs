module AOC.Core.Input

open System

let splitEachChar (s: string) = s |> Seq.map string

let splitEachNewline (s: string) = s.Trim().Split(Environment.NewLine) |> seq

let splitEach (sep: string) (s: string) = s.Split(sep) |> seq