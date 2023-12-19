type Coord* = tuple[x: int, y: int]

const ZERO*: Coord = (x: 0, y: 0)
const UP*: Coord = (x: 0, y: -1)
const DOWN*: Coord = (x: 0, y: 1)
const LEFT*: Coord = (x: -1, y: 0)
const RIGHT*: Coord = (x: 1, y: 0)
const NORTH*: Coord = UP
const SOUTH*: Coord = DOWN
const WEST*: Coord = LEFT
const EAST*: Coord = RIGHT

func `+`*(a: Coord, b: Coord): Coord =
    return (a.x + b.x, a.y + b.y)

func `-`*(a: Coord, b: Coord): Coord =
    return (a.x - b.x, a.y - b.y)

func to*(a: Coord, b: Coord): seq[Coord] =
    var cells: seq[Coord] = @[]
    var x: int = a.x
    var y: int = a.y
    while x != b.x:
        cells.add((x, y))
        x += (if x < b.x: 1 else: -1)
    while y != b.y:
        cells.add((x, y))
        y += (if y < b.y: 1 else: -1)
    return cells

