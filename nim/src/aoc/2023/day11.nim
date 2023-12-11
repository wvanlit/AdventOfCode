import "../utils/io_utils", "../utils/seq_utils"
include "../ds/grid"
import algorithm, sequtils, strutils, tables, math, sugar, strformat

func parseGrid(input: seq[string]): Grid[string] =
    var grid = initGrid[string](input[0].len, input.len)
    for y, line in input:
        for x, c in line:
            grid.setValue(x, y, $c)
    return grid

func shiftGalaxies(grid: Grid[string], shiftAmount: int): seq[Cell[string]] =
    result = @[]

    # Get empty columns + rows
    var emptyColumns = newSeq[int]()
    var emptyRows = newSeq[int]()

    for x in 0..<grid.width:
        var isEmpty = true
        for y in 0..<grid.height:
            if grid.getValue(x, y) == "#":
                isEmpty = false
        if isEmpty: emptyColumns.add(x)
    
    for y in 0..<grid.height:
        var isEmpty = true
        for x in 0..<grid.width:
            if grid.getValue(x, y) == "#":
                isEmpty = false
        if isEmpty: emptyRows.add(y)

    var galaxies = grid.iterate.toSeq.filterIt(it.value == "#")

    for g in galaxies:
        var xs = emptyColumns.filterIt(it <= g.x).len
        var ys = emptyRows.filterIt(it <= g.y).len

        let x = g.x + (xs * (shiftAmount - 1))
        let y = g.y + (ys * (shiftAmount - 1))

        result.add((x, y, "G"))

func manhattanDistance(p1: Cell[string], p2: Cell[string]): int =
    abs(p1.x - p2.x) + abs(p1.y - p2.y)

func totalDistance(galaxies: seq[Cell[string]]): int =
    galaxies.pairs().mapIt(manhattanDistance(it[0], it[1])).sum

proc main() =
    let input = readInput(2023, 11, test=false).strip.splitLines
    let grid = parseGrid(input)

    echo "Part 1: ", totalDistance(grid.shiftGalaxies(2))
    echo "Part 2: ", totalDistance(grid.shiftGalaxies(1000000))

main()