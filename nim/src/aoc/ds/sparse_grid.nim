import tables
import "./coord"
import options, math

type Cell*[T] = tuple[coord: Coord, value: T]
type Bounds* = tuple[minX: int, minY: int, maxX: int, maxY: int]

type SparseGrid*[T] = object
    data*: Table[Coord, T]
    default: T
    boundsDirty: bool
    bounds: Bounds

proc initSparseGrid*[T](default: T): SparseGrid[T] =
    result.data = initTable[Coord, T]()
    result.default = default

    result.boundsDirty = true
    result.bounds = (0, 0, 0, 0)

func `[]`*[T](grid: SparseGrid[T], x: int, y: int): T =
    grid.data.getOrDefault((x, y), grid.default)

func `[]=`*[T](grid:var SparseGrid[T], x: int, y: int, value: T) =
    grid.data[(x, y)] = value
    grid.boundsDirty = true

func `[]`*[T](grid: SparseGrid[T], coord: Coord): T =
    grid.data.getOrDefault(coord, grid.default)

func `[]=`*[T](grid:var SparseGrid[T], coord: Coord, value: T) =
    grid.data[coord] = value
    grid.boundsDirty = true

func calculateBounds*[T](grid: var SparseGrid[T]) =
    var minX, minY = int.high
    var maxX, maxY = int.low
    for (x, y) in grid.data.keys:
        minX = min(minX, x)
        maxX = max(maxX, x)
        minY = min(minY, y)
        maxY = max(maxY, y)

    grid.boundsDirty = false
    grid.bounds = (minX, minY, maxX, maxY)

func bounds*[T](grid: SparseGrid[T]): tuple[minX: int, minY: int, maxX: int, maxY: int] =
    assert grid.boundsDirty == false, "Bounds are dirty, call calculateBounds after modifying the grid"
    grid.bounds

func getRow*[T](grid: SparseGrid[T], y: int): seq[Cell] =
    let bounds = grid.bounds
    for x in bounds.minX .. bounds.maxX:
        let value = grid[x, y]
        if value != grid.default:
            result.add((x, y, value))

func getCol*[T](grid: SparseGrid[T], x: int): seq[Cell] =
    let bounds = grid.bounds
    for y in bounds.minY .. bounds.maxY:
        let value = grid[x, y]
        if value != grid.default:
            result.add((x, y, value))

func raycast*[T](grid: SparseGrid[T], start: Coord, direction: Coord): Cell[T] =
    var (x, y) = start
    var (dx, dy) = direction
    while true:
        let value = grid[x, y]
        if value != grid.default:
            return ((x, y), value)
        x += dx
        y += dy
        if x < grid.bounds.minX or x > grid.bounds.maxX or y < grid.bounds.minY or y > grid.bounds.maxY:
            return ((x,y), grid.default)

func inBounds*(grid: SparseGrid, coord: Coord): bool =
    let (x, y) = coord
    let bounds = grid.bounds
    x >= bounds.minX and x <= bounds.maxX and y >= bounds.minY and y <= bounds.maxY