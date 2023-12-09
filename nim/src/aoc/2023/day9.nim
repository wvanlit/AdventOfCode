import "../utils/io_utils", "../utils/seq_utils"
import algorithm, sequtils, strutils, tables, math, sugar

func generateSequenceList(sequence: seq[int]): seq[seq[int]] =
    result = @[sequence]
    while not result[^1].allIt(it == 0):
        result.add(result[^1].window(2).mapIt(it[1] - it[0]).toSeq)

func extrapolateNextNumber(sequence: seq[int]): int =
    generateSequenceList(sequence).reversed.mapIt(it[^1]).foldl(a + b)

func extrapolatePreviousNumber(sequence: seq[int]): int =
    generateSequenceList(sequence).reversed.mapIt(it[0]).foldl(b - a)

proc main() =
    let input = readInput(2023, 9, test = false).strip.splitLines
    let sequences = input.mapIt(it.splitWhitespace.mapIt(it.parseInt).toSeq).toSeq

    echo "Part 1: ", sequences.map(extrapolateNextNumber).sum
    echo "Part 2: ", sequences.map(extrapolatePreviousNumber).sum

main()
