import "../utils/io_utils", "../utils/seq_utils"
import algorithm, sequtils, strutils, tables, math, sugar

proc extrapolateNextNumber(sequence: seq[int]): int =
    var lists = @[sequence]

    while not lists[^1].allIt(it == 0):
        lists.add(lists[^1].window(2).mapIt(it[1] - it[0]).toSeq)

    for last in lists.reversed().mapIt(it[^1]):
        result = last + result

proc extrapolatePreviousNumber(sequence: seq[int]): int =
    var lists = @[sequence]

    while not lists[^1].allIt(it == 0):
        lists.add(lists[^1].window(2).mapIt(it[1] - it[0]).toSeq)

    for first in lists.reversed().mapIt(it[0]):
        result = first - result

proc main() =
    let input = readInput(2023, 9, test = false).strip
    let sequences = input.splitLines.mapIt(it.splitWhitespace.mapIt(it.parseInt).toSeq).toSeq

    echo "Part 1: ", sequences.map(extrapolateNextNumber).sum
    echo "Part 2: ", sequences.map(extrapolatePreviousNumber).sum

main()