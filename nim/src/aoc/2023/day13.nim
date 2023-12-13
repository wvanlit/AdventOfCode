import "../utils/io_utils", "../utils/seq_utils"
include "../ds/grid"
import algorithm, sequtils, strutils, parseutils, tables, math, sugar, strformat

const INVALID = (-1, -1)

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

    let c1 = s1.mapIt(if checkRow: grid.row(it) else: grid.col(it)).flatten
    let c2 = s2.mapIt(if checkRow: grid.row(it) else: grid.col(it)).flatten

    if c1.join("") == c2.join(""): 
        return true
    return false

func checkVerticalSymmetry(grid: Grid[char]): seq[tuple[a: int, b: int]] =
    let pairs = generateSymmetryChecks(grid.width)
    
    for p in pairs:
        if grid.checkSymmetry(p, checkRow=false): 
            result.add((p.a.b, p.b.a))

func checkHorizontalSymmetry(grid: Grid[char]) : seq[tuple[a: int, b: int]] =
    let pairs = generateSymmetryChecks(grid.height)
    for p in pairs:
        if grid.checkSymmetry(p, checkRow=true): 
            result.add((p.a.b, p.b.a))

func checkCleanSymmetry(grid: Grid[char]): int =
    let vertical = checkVerticalSymmetry(grid)
    let horizontal = checkHorizontalSymmetry(grid)

    assert(not (vertical.len == 0 and horizontal.len == 0), "No symmetry found")

    if vertical.len != 0: # Vertical symmetry
        return vertical[0].a + 1 # Columns to the left of the symmetry line
    else: # Horizontal symmetry
        return (horizontal[0].a + 1) * 100# Rows above the symmetry line

func checkSmudgedSymmetry(grid: var Grid[char]): int =
    let vclean = checkVerticalSymmetry(grid)
    let hclean = checkHorizontalSymmetry(grid)

    for v in grid.iterate():
        grid.setValue(v.x, v.y, if v.value == '1': '0' else: '1')
        
        let vsmudge = checkVerticalSymmetry(grid).filterIt((it.a, it.b) notin vclean)
        let hsmudge = checkHorizontalSymmetry(grid).filterIt((it.a, it.b) notin hclean)

        if vsmudge.len >= 1:
            return vsmudge[0].a + 1 
        if hsmudge.len >= 1:
            return (hsmudge[0].a + 1) * 100
        grid.setValue(v.x, v.y, v.value)

proc main() =
    var input = readInput(2023, 13, test=false).strip.split("\n\n").map(parse)
    
    var p1 = 0
    var p2 = 0
    for g in input.mitems:
        p1 += checkCleanSymmetry(g)
        p2 += checkSmudgedSymmetry(g)

    echo "Part 1: ", p1
    echo "Part 2: ", p2

main()