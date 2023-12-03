include "../prelude"
include "../ds/grid" # TODO change to import

import math

type PartNumber = tuple[n: int, cells: seq[tuple[x: int, y: int]]]
type ValidPart = tuple[n: int, symbols: seq[Cell[char]]]
type Gear = (int, int)

proc partNumbers(grid: Grid[char]) : seq[PartNumber] =
    result = @[]

    var currentCells: seq[(int, int)] = @[] 
    var currentNum = ""

    var prevCell = (0, 0)

    for cell in grid.iterate():
        if cell.value.isDigit:
            currentNum.add(cell.value)
            currentCells.add((cell.x, cell.y))
        elif currentNum != "":
            result.add((currentNum.parseInt, currentCells))
            currentNum = ""
            currentCells = @[]

        prevCell = (cell.x, cell.y)

proc filterValidParts(grid: Grid[char], parts: seq[PartNumber]) : seq[ValidPart] =
    result = @[]

    # TODO This can be written more functional
    for part in parts:
        let possibleCells = part.cells.mapIt(grid.getNeighbors(it.x, it.y)).concat
        let symbols = possibleCells.filterIt(not it.value.isDigit and it.value != '.').deduplicate

        if symbols.len != 0:
            result.add((part.n.int, symbols))

proc filterGears(parts: seq[ValidPart]): seq[Gear] =
    result = @[]

    let gears : seq[Cell[int]] = parts
        .map(p => p.symbols
            .filterIt(it.value == '*')
            .mapIt((it.x, it.y, p.n)))
        .concat

    var countTable = initCountTable[(int, int)]()

    for gear in gears:
        countTable.inc((gear.x, gear.y))
    
    for (cell, count) in countTable.pairs:
        if count == 2:
            let parts = gears.filterIt(it.x == cell[0] and it.y == cell[1])
            
            assert parts.len == 2

            result.add((parts[0].value, parts[1].value))


let lines = readInput(2023, 3, test=false).strip.splitLines()

var grid = initGrid[char](lines.len, lines[0].len)

for (r, row) in lines.pairs:
    for (c, chr) in row.pairs:
        setValue[char](grid, c, r, chr)

let parts = partNumbers(grid)
let validParts = filterValidParts(grid, parts)
let gears = filterGears(validParts)

echo "Part 1: ", validParts.mapIt(it.n).sum
echo "Part 2: ", gears.mapIt(it[0] * it[1]).sum
