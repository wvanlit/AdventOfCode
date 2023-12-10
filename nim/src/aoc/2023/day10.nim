import "../utils/io_utils"
include "../ds/grid"
import algorithm, sequtils, strutils, tables, math, sugar

type Pipe = enum NS, EW, NE, NW, SW, SE, NO, ST

func parseGrid(input: seq[string]): Grid[Pipe] =
    let width = input[0].len
    let height = input.len

    result = initGrid[Pipe](width, height)

    for i, row in input:
        for j, ch in row:
            result.setValue(j, i, case ch
                of '|': Pipe.NS
                of '-': Pipe.EW
                of 'L': Pipe.NE
                of 'J': Pipe.NW
                of '7': Pipe.SW
                of 'F': Pipe.SE
                of '.': Pipe.NO
                of 'S': Pipe.ST
                else: raise newException(ValueError, "Invalid character: " & $ch))

func pipeToChar(pipe: Pipe): string =
    case pipe
        of Pipe.ST: "┼"
        of Pipe.NO: " "
        of Pipe.NS: "│"
        of Pipe.NE: "└"
        of Pipe.NW: "┘"
        of Pipe.EW: "─"
        of Pipe.SW: "┐"
        of Pipe.SE: "┌"

func findStartPos(grid: Grid[Pipe]): tuple[x: int, y: int] =
    for x, y, cell in grid.iterate():
        if cell == Pipe.ST:
            return (x, y)
    raise newException(ValueError, "No start position found")

func connections(pipe: Pipe): seq[tuple[x: int, y: int]] =
    case pipe
        of Pipe.ST: @[(0, 1),(1, 0),(0, -1),(-1, 0)]
        of Pipe.NO: @[]
        of Pipe.NS: @[(0, 1), (0, -1)]
        of Pipe.EW : @[(1, 0), (-1, 0)]
        of Pipe.NW: @[(0, -1), (-1, 0)]
        of Pipe.SE: @[(0, 1), (1, 0)]
        of Pipe.NE: @[(1, 0), (0, -1)]
        of Pipe.SW: @[(0, 1), (-1, 0)]

func canConnect(p1: Cell[Pipe], p2: Cell[Pipe]): bool =
    let c1 = connections(p1.value).mapIt((p1.x + it.x, p1.y + it.y))
    let c2 = connections(p2.value).mapIt((p2.x + it.x, p2.y + it.y))
    
    return c1.contains((p2.x, p2.y)) and c2.contains((p1.x, p1.y))

proc findFarthestFromStart(grid: Grid[Pipe]): int =
    var distances = initTable[tuple[x: int, y: int], int]()
    let start = findStartPos(grid)

    distances[start] = 0
    var queue = @[start]

    while queue.len > 0:
        let (x, y) = queue.pop
        let pipeType = grid.getValue(x, y)
        let currCell: Cell[Pipe] = (x, y, pipeType)
        
        let currDist = distances[(x, y)]
        
        let newDist = currDist + 1

        for c in grid.getDirectNeighbors(x, y):
            if not canConnect(currCell, c):
                continue

            if distances.hasKey((c.x, c.y)):
                if distances[(c.x, c.y)] <= newDist:
                    continue
                    
            queue.add((c.x, c.y))
            distances[(c.x, c.y)] = newDist

    return distances.values.toSeq.max

func floodFill(grid: Grid[Pipe]): seq[Cell[Pipe]] =
    result = @[]
    let start = findStartPos(grid)
    var queue = @[start]

    while queue.len > 0:
        let (x, y) = queue.pop
        let pipeType = grid.getValue(x, y)
        let currCell: Cell[Pipe] = (x, y, pipeType)
        
        result.add(currCell)

        for c in grid.getDirectNeighbors(x, y):
            if not canConnect(currCell, c):
                continue

            if result.contains(c):
                continue
                    
            queue.add((c.x, c.y))

let roundedCorners = {
    (Pipe.SE, Pipe.SW): 2, # ┌┐ - 2 hits
    (Pipe.NE, Pipe.NW): 2, # └┘ - 2 hits
    (Pipe.SE, Pipe.NW): 1, # ┌┘ - 1 hit
    (Pipe.NE, Pipe.SW): 1, # └┐ - 1 hit
    # TODO: handle start node correctly
    # ███████████████████┌┘└┐┌┘└┘│└┐│┌┐│││┌┘┌┘┌┘█└┐│└┐┌┘└┐┌┘┌┐┌┐┌┘│┌┐│││││└┐└┐└┘└┐┌┘└┘└┼└─┐┌┘ ┌┘└┘└┐└┘│┌─┘ ┌┐│┌┘└┐┌───┐┌┘    
}.toTable

proc inPolygon(grid: Grid[Pipe], pipe: seq[Cell[Pipe]], cell: Cell[Pipe]): bool =
    var hits = 0

    var raycast = (cell.x+1)..(grid.width - 1)
    var prev = Pipe.NO
    
    for rx in raycast:
        let c = grid.getValue(rx, cell.y)

        if c == Pipe.NO:
            prev = c
            continue
        if c == Pipe.EW: # Horizontal pipe
            continue
        
        if c == Pipe.NS: # Vertical pipe
            hits += 1
        elif roundedCorners.hasKey((prev, c)):
            hits += roundedCorners[(prev, c)]

        prev = c

    return not hits mod 2 == 0

proc findEnclosedArea(grid:var Grid[Pipe]): int =
    let pipeCells = grid.floodFill()

    # Remove all pipes not in the main pipe
    for y in 0..<grid.height:
        for x in 0..<grid.width:
            let cell = grid.getValue(x, y)
            if cell != Pipe.NO and not pipeCells.anyIt(it.x == x and it.y == y):
                grid.setValue(x, y, Pipe.NO)

    let cellsInPipeArea = grid.iterate().toSeq.filterIt(it.value == Pipe.NO).filterIt(inPolygon(grid, pipeCells, it))

    # The pipe is similar to a polygon
    for y in 0..<grid.height:
        for x in 0..<grid.width:
            if pipeCells.anyIt(it.x == x and it.y == y):
                stdout.write(pipeToChar(grid.getValue(x, y)))
            elif cellsInPipeArea.anyIt(it.x == x and it.y == y):
                stdout.write("█")
            else:
                stdout.write(" ")

        stdout.write("\n")
    stdout.flushFile()    

    return cellsInPipeArea.len

proc main() =
    let input = readInput(2023, 10, test = false).strip.splitLines
    var grid = parseGrid(input)

    echo "Part 1: ", findFarthestFromStart(grid)
    echo "Part 2: ", findEnclosedArea(grid)

main()
