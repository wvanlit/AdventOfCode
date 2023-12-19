import "../utils/io_utils", "../utils/seq_utils"
import "../ds/sparse_grid", "../ds/coord"
import strutils, strformat, sequtils, math, tables, algorithm, sugar, os

type Tile = enum Empty, MirrorLeft, MirrorRight, SplitterHorizontal, SplitterVertical

func parse(input: string): SparseGrid[Tile] =
    result = initSparseGrid[Tile](Tile.Empty)
    for y, line in input.splitLines.toSeq:
        for x, c in line:
            let tile = case c:
                of '.' : Tile.Empty
                of '\\': Tile.MirrorLeft
                of '/' : Tile.MirrorRight
                of '-' : Tile.SplitterHorizontal
                of '|' : Tile.SplitterVertical
                else: raise newException(ValueError, "Invalid tile: " & $c)
                
            if tile != Tile.Empty:
                result[x, y] = tile
    result.calculateBounds

func `$`(tile: Tile): string =
    case tile:
        of Tile.Empty: return "."
        of Tile.MirrorLeft: return "\\"
        of Tile.MirrorRight: return "/"
        of Tile.SplitterHorizontal: return "-"
        of Tile.SplitterVertical: return "|"

type Ray = tuple[origin: Coord, direction: Coord]

func `$`(ray: Ray): string =
    var dir = "UNKNOWN"
    if ray.direction == UP:
        dir = "^"
    elif ray.direction == DOWN:
        dir = "v"
    elif ray.direction == LEFT:
        dir = "<"
    elif ray.direction == RIGHT:
        dir = ">"
    return fmt"[{ray.origin} {dir}]"

func calculateEnergizedNodes(grid: SparseGrid[Tile], start: Ray): int =
    var visited = CountTable[Ray]()
    var stack: seq[Ray] = @[start]

    proc print() =
        let energizedNodes = visited.keys.toSeq.mapIt(it.origin).deduplicate
        let bounds = grid.bounds
        for y in bounds.minY .. bounds.maxY:
            # Print Grid
            for x in bounds.minX .. bounds.maxX:
                let c = (x, y)
                stdout.write($grid[c])
            stdout.write("   ")
            # Print Energized
            for x in bounds.minX .. bounds.maxX:
                let c = (x, y)
                if c in energizedNodes:
                    stdout.write("#")
                else:
                    stdout.write($grid[c])
            stdout.write("\n")
        stdout.flushFile

    while stack.len > 0:
        let ray = stack.pop
        if ray in visited: # Found a loop
            continue

        let next = grid.raycast(ray.origin + ray.direction, ray.direction)

        # print()
        # echo "--------------------------"
        # echo fmt"{ray} to [{next.coord.x}, {next.coord.y}] ({next.value})"

        for c in ray.origin.to(next.coord):
            visited.inc((c, ray.direction))

        if next.value == Tile.Empty:
            continue
        elif next.value == Tile.SplitterHorizontal:
            if ray.direction == LEFT or ray.direction == RIGHT:
                stack.add((next.coord, ray.direction))
            else:
                stack.add((next.coord, LEFT))
                stack.add((next.coord, RIGHT))
        elif next.value == Tile.SplitterVertical:
            if ray.direction == UP or ray.direction == DOWN:
                stack.add((next.coord, ray.direction))
            else:
                stack.add((next.coord, UP))
                stack.add((next.coord, DOWN))
        elif next.value == Tile.MirrorLeft:
            let direction =
                if ray.direction == UP: LEFT
                elif ray.direction == DOWN: RIGHT
                elif ray.direction == LEFT: UP
                elif ray.direction == RIGHT: DOWN
                else: raise newException(ValueError, "Invalid direction: " & $ray.direction)
            stack.add((next.coord, direction))
        elif next.value == Tile.MirrorRight:
            let direction =
                if ray.direction == UP: RIGHT
                elif ray.direction == DOWN: LEFT
                elif ray.direction == LEFT: DOWN
                elif ray.direction == RIGHT: UP
                else: raise newException(ValueError, "Invalid direction: " & $ray.direction)
            stack.add((next.coord, direction))
        else:
            raise newException(ValueError, "Invalid tile: " & $next.value)
            
    visited.keys.toSeq.filterIt(grid.inBounds(it.origin)).mapIt(it.origin).deduplicate.len

proc findEnergized(grid: SparseGrid[Tile]): int =
    calculateEnergizedNodes(grid, (LEFT, RIGHT))
    

proc findOptimalStartingPosition(grid: SparseGrid[Tile]): int =
    result = int.low

    # Test all borders
    for x in grid.bounds.minX .. grid.bounds.maxX:
        result = max(result, calculateEnergizedNodes(grid, ((x, grid.bounds.minY - 1), DOWN)))
        result = max(result, calculateEnergizedNodes(grid, ((x, grid.bounds.maxY + 1), UP)))

    for y in grid.bounds.minY .. grid.bounds.maxY:
        result = max(result, calculateEnergizedNodes(grid, ((grid.bounds.minX - 1, y), RIGHT)))
        result = max(result, calculateEnergizedNodes(grid, ((grid.bounds.maxX + 1, y), LEFT)))

proc main() =
    let input = readInput(2023, 16, test=false).strip
    let grid = parse(input)

    echo "Part 1: ", findEnergized(grid)
    echo "Part 2: ", findOptimalStartingPosition(grid)

main()