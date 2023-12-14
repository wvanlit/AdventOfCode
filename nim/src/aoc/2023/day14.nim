import "../utils/io_utils", "../utils/seq_utils"
include "../ds/grid"
import algorithm, sequtils, strutils, parseutils, tables, math, sugar, strformat, os, times

const EMPTY = '.'
const ROUND = 'O'
const CUBE = '#'

type DIR = enum NORTH SOUTH EAST WEST

proc parse(input: string): Grid[char] =
    let gridLines = input.splitLines
    result = initGrid[char](len(gridLines[0]), len(gridLines))
    for y, line in gridLines:
        for x, c in line:
            result.setValue(x, y, c) 

## I could be smart and make this generic, but it is early in the morning and I'm tired

func updateTile(grid: var Grid[char], c: Cell[char], dir: DIR): bool = # Returns true if the grid changed
    if c.value == EMPTY or c.value == CUBE:
        return false
    
    if c.value == ROUND: # These shift towards the direction
        case dir:
            of NORTH:
                if c.y == 0 or grid.getValue(c.x, c.y - 1) != EMPTY: # Can't move
                    return false
                else:
                    grid.setValue(c.x, c.y, EMPTY)
                    grid.setValue(c.x, c.y - 1, ROUND)
                    return true
            of SOUTH:
                if c.y == grid.height - 1 or grid.getValue(c.x, c.y + 1) != EMPTY: # Can't move
                    return false
                else:
                    grid.setValue(c.x, c.y, EMPTY)
                    grid.setValue(c.x, c.y + 1, ROUND)
                    return true
            of WEST:
                if c.x == 0 or grid.getValue(c.x - 1, c.y) != EMPTY: # Can't move
                    return false
                else:
                    grid.setValue(c.x, c.y, EMPTY)
                    grid.setValue(c.x - 1, c.y, ROUND)
                    return true
            of EAST:
                if c.x == grid.width - 1 or grid.getValue(c.x + 1, c.y) != EMPTY: # Can't move
                    return false
                else:
                    grid.setValue(c.x, c.y, EMPTY)
                    grid.setValue(c.x + 1, c.y, ROUND)
                    return true

    assert false, "Invalid character in grid: " & $c

func simulateTiltStepNorth(grid: var Grid[char]): bool =
    for y in 0..<grid.height: # Iterate from top to bottom
        for x in 0..<grid.width:
            if updateTile(grid, grid.getCell(x, y), NORTH):
                result = true

func simulateTiltStepSouth(grid: var Grid[char]): bool = 
    for y in (0..<grid.height).toSeq.reversed: # Iterate from bottom to top
        for x in 0..<grid.width:
            if updateTile(grid, grid.getCell(x, y), SOUTH):
                result = true

func simulateTiltStepWest(grid: var Grid[char]): bool = 
    for x in 0..<grid.width: # Iterate from left to right
        for y in 0..<grid.height:
            if updateTile(grid, grid.getCell(x, y), WEST):
                result = true

func simulateTiltStepEast(grid: var Grid[char]): bool = 
    for x in (0..<grid.width).toSeq.reversed: # Iterate from right to left
        for y in 0..<grid.height:
            if updateTile(grid, grid.getCell(x, y), EAST):
                result = true

func scoreGrid(grid: Grid[char]): int =
    result = 0
    for c in grid.iterate:
        if c.value == ROUND:
            let score = grid.height - c.y
            result += score

func simulateGridUntilStable(grid: var Grid[char]): int =
    while simulateTiltStepNorth(grid):
        discard
    result = scoreGrid(grid)

func simulateFullStep(grid: var Grid[char]): void =
    while simulateTiltStepNorth(grid):
        discard
    while simulateTiltStepWest(grid):
        discard
    while simulateTiltStepSouth(grid):
        discard
    while simulateTiltStepEast(grid):
        discard

func strrep(grid: Grid[char]): string = grid.data.flatten.join("").strip

proc doesItCycle(states: seq[Grid[char]]): bool =
    for i in 0..<states.len:
        var grid = states[i]
        simulateFullStep(grid)
        
        let gridToCheckAgainst = states[(i + 1) mod states.len]

        if grid.strrep != gridToCheckAgainst.strrep:
            return false

    return true

proc findCycleCount(grid: Grid[char]): tuple[cycleInterval: int, afterIterations: int, cycleStates: Table[int, Grid[char]]] =
    result = (-1, -1, Table[int, Grid[char]]())

    var iteration = 0

    var memo = Table[string, Grid[char]]()
    var currState = grid.strrep
    var editableGrid = grid
    var cycleState = newSeq[Grid[char]]() 

    while not doesItCycle(cycleState) or not memo.hasKey(currState):
        currState = editableGrid.strrep

        if not memo.hasKey(currState): # We haven't seen this state before
            cycleState = @[]

        cycleState.add(editableGrid)

        memo[currState] = editableGrid
        iteration += 1

        simulateFullStep(editableGrid)

    var iterCache = Table[int, Grid[char]]()
    for i, state in cycleState.pairs:
        iterCache[i] = state

    result.cycleInterval = cycleState.len
    result.afterIterations = iteration - cycleState.len
    result.cycleStates = iterCache

proc simulateGridForNCycles(grid: var Grid[char], n: int): int =
    var (cycleInterval, afterIterations, cycleStates) = findCycleCount(grid)
    echo "Cycle interval: ", cycleInterval
    echo "After iterations: ", afterIterations
    echo "Cycle states: ", cycleStates.len

    var desiredState = cycleStates[(n - afterIterations) mod cycleInterval]

    result = scoreGrid(desiredState)

proc main() =
    let input = readInput(2023, 14, test=false).strip

    var grid1 = parse(input)
    echo "Part 1: ", simulateGridUntilStable(grid1)
    var grid2 = parse(input)
    echo "Part 2: ", simulateGridForNCycles(grid2, 1000000000)

if isMainModule:
    main()  