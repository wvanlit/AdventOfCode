import sequtils, sugar

type
    Grid[T] = object
        data: seq[seq[T]]
        width, height: int
    Cell[T] = tuple[x: int, y: int, value: T]

proc initGrid[T](width: int, height: int): Grid[T] =
    result.width = width
    result.height = height
    result.data = newSeqWith(height, newSeq[T](width))

proc setValue[T](grid: var Grid[T], x: int, y: int, value: T) =
    if x >= 0 and x < grid.width and y >= 0 and y < grid.height:
        grid.data[y][x] = value
    else:
        assert false, "Index (" & $x & "," & $y & ") out of bounds"

proc getValue[T](grid: Grid[T], x: int, y: int): T =
    if x >= 0 and x < grid.width and y >= 0 and y < grid.height:
        return grid.data[y][x]
    assert false, "Index (" & $x & "," & $y & ") out of bounds"

proc printGrid[T](grid: Grid[T]) =
    for y in 0..<grid.height:
        for x in 0..<grid.width:
            stdout.write(grid.data[y][x])
        stdout.write("\n")
    stdout.flushFile()

proc printGrid[T](grid: Grid[T], str: T -> string) =
    for y in 0..<grid.height:
        for x in 0..<grid.width:
            stdout.write(str(grid.data[y][x]))
        stdout.write("\n")
    stdout.flushFile()

proc getNeighbors[T](grid: Grid[T], x: int, y: int): seq[Cell[T]] =
    let dx = [-1, 0, 1, -1, 1, -1, 0, 1]
    let dy = [-1, -1, -1, 0, 0, 1, 1, 1]
    result = newSeq[(int, int, T)]()
    for i in 0..7:
        let nx = x + dx[i]
        let ny = y + dy[i]
        if nx >= 0 and nx < grid.width and ny >= 0 and ny < grid.height:
            result.add((nx, ny, grid.data[ny][nx]))

proc getDirectNeighbors[T](grid: Grid[T], x: int, y: int): seq[Cell[T]] =
    let dx = [0, 1, 0, -1]
    let dy = [-1, 0, 1, 0]
    result = newSeq[(int, int, T)]()
    for i in 0..3:
        let nx = x + dx[i]
        let ny = y + dy[i]
        if nx >= 0 and nx < grid.width and ny >= 0 and ny < grid.height:
            result.add((nx, ny, grid.data[ny][nx]))

iterator iterate[T](grid: Grid[T]): Cell[T] =
    for y in 0..<grid.height:
        for x in 0..<grid.width:
            yield (x, y, grid.data[y][x])
