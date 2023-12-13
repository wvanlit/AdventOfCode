import "../utils/io_utils", "../utils/seq_utils"
include "../ds/grid"
import algorithm, sequtils, strutils, tables, math, sugar, strformat

func parse(line: string): Grid[char] =
    let gridLines = line.split("\n")
    result = initGrid[char](len(gridLines[0]), len(gridLines))
    for y, line in gridLines:
        for x, c in line:
            result.setValue(x, y, if c == '#': '1' else: '0') 

func generateSymmetryChecks(length: int): seq[tuple[a: Slice[int], b: Slice[int]]] =
    let pairs = (0..length-1).toSeq.window(2).mapIt((it[0], it[1]))

    result = @[]
    for i, (x1, x2) in pairs:
        let half = min(x1, (length - 1) - x2) # Only check until either is out of bounds
        let r1 = (x1-half..x1)
        let r2 = (x2..x2+half)
        result.add((r1, r2))

func checkSymmetry(grid: Grid[char], indices: tuple[a: Slice[int], b: Slice[int]], checkRow = false): bool =
    let (r1, r2) = indices
    let s1 = r1.toSeq
    let s2 = r2.toSeq.reversed()

    let b1 = s1.mapIt(if checkRow: grid.row(it) else: grid.col(it)).flatten.join("")
    let b2 = s2.mapIt(if checkRow: grid.row(it) else: grid.col(it)).flatten.join("")

    if b1 == b2: 
        return true
    return false

func checkVerticalSymmetry(grid: Grid[char]): tuple[a: int, b: int] =
    let pairs = generateSymmetryChecks(grid.width)
    
    for p in pairs:
        if grid.checkSymmetry(p, checkRow=false): 
            return (p.a.b, p.b.a)
    return (0, 0)

func checkHorizontalSymmetry(grid: Grid[char]) : tuple[a: int, b: int] =
    let pairs = generateSymmetryChecks(grid.height)
    for p in pairs:
        if grid.checkSymmetry(p, checkRow=true): 
            return (p.a.b, p.b.a)
    return (0, 0)

func checkSingleSymmetry(grid: Grid[char]): int =
    let vertical = checkVerticalSymmetry(grid)
    let horizontal = checkHorizontalSymmetry(grid)

    assert(not (vertical.a == 0 and vertical.b == 0 and horizontal.a == 0 and horizontal.b == 0), "No symmetry found")

    if vertical != (0,0): # Vertical symmetry
        return vertical.a + 1 # Columns to the left of the symmetry line
    else: # Horizontal symmetry
        return (horizontal.a + 1) * 100# Rows above the symmetry line

proc main() =
    let input = readInput(2023, 13, test=false).strip.split("\n\n").map(parse)
    
    echo "Part 1: ", input.map(checkSingleSymmetry).sum
    echo "Part 2: ", -1

main()