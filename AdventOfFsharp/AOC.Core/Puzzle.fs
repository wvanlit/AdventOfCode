module AOC.Core.Puzzle

type Part = string -> string
type Day = Part * Part
type Year = Map<uint, Day>
type Solutions = Map<uint, Year>

let unsolved = fun input -> "TODO"

// Just here to copy over
let dayUnsolved : Day = (unsolved, unsolved)
