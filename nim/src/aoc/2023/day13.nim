import "../utils/io_utils", "../utils/seq_utils"
include "../ds/grid"
import algorithm, sequtils, strutils, tables, math, sugar, strformat

func parse(line: string): Grid[char] =
    let gridLines = line.split("\n")
    result = initGrid[char](len(gridLines[0]), len(gridLines))
    for y, line in gridLines:
        for x, c in line:
            result.setValue(x, y, c) 

func checkVerticalSymmetry(grid: Grid[char]): bool =
    let pairs = (0..grid.width).toSeq.window(2).mapIt((it[0], it[1]))
    let check = ((1..4).toSeq, (5..9).toSeq.reversed)

    for i, (x1, x2) in check[0].zip(check[1]):
        # TODO: check if this is correct



func checkHorizontalSymmetry(grid: Grid[char]): bool =
    let pairs = (0..grid.height).toSeq.window(2).mapIt((it[0], it[1]))

func checkSymmetry(grid: Grid[char]): bool =
    if checkVerticalSymmetry(grid):
        return true
    elif checkHorizontalSymmetry(grid):
        return true
    else:
        return false

proc main() =
    let input = readInput(2023, 13, test=true).strip.split("\n\n").map(parse)
    
    for i in input:
        i.printGrid()
        echo i.checkSymmetry()

    echo "Part 1: ", -1
    echo "Part 2: ", -1

main()