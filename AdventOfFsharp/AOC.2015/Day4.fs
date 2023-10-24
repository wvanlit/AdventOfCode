module AOC.Year2015.Day4

open System
open System.Security.Cryptography
open System.Text
open AOC.Core.Puzzle

let infiniteLoopPrevention = 10_000_000

let computeMD5 (input: string) : string =
    use md5 = MD5.Create()
    let bytes = Encoding.UTF8.GetBytes(input)
    let hashBytes = md5.ComputeHash bytes
    let hash = BitConverter.ToString(hashBytes).Replace("-", "")
    hash

let part1 (input: string) =
    let hash (n: int) = computeMD5 $"{input.Trim()}{n}"
    let startsWith5Zeroes (s: string) = s.StartsWith "00000"

    let rec findHash n =
        if hash n |> startsWith5Zeroes || n >= infiniteLoopPrevention then
            n
        else
            findHash (n + 1)

    findHash 1

let part2 (input: string) =
    let hash (n: int) = computeMD5 $"{input.Trim()}{n}"
    let startsWith6Zeroes (s: string) = s.StartsWith "000000"

    let rec findHash n =
        if hash n |> startsWith6Zeroes || n >= infiniteLoopPrevention then
            n
        else
            findHash (n + 1)

    findHash 1
    
let day: Day = (part1 >> string, part2 >> string)
