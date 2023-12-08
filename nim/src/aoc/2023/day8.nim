import "../utils/io_utils"
import sequtils, strutils, tables, math, sugar

type Path = seq[char]
type Node = tuple[id: string, left: string, right: string]
type Nodes = Table[string, Node]

func parse(input: string): tuple[path: Path, nodes: Nodes] =
    let p = input.split("\n\n")

    result.path = p[0].strip.toSeq
    result.nodes = initTable[string, Node]()

    for line in p[1].splitLines:
        let parts = line.replace(" ", "").split("=")
        let id = parts[0].strip
        let connections = parts[1].strip[1..^2].split(",").mapIt(it.strip)
        let left = connections[0].strip
        let right = connections[1].strip

        result.nodes[id] = (id, left, right)

proc part1(path: Path, nodes: Nodes, start: string = "AAA", exitCondition: string -> bool): int =
    var currentNode = nodes[start]
    var steps = 0

    while not exitCondition(currentNode.id):
        let dir = path[steps mod path.len] # Prevents index out of bounds
        currentNode = if dir == 'L': nodes[currentNode.left] else: nodes[currentNode.right]
        inc(steps)

    return steps

proc part2(path: Path, nodes: Nodes): int =
    var currentNodes = nodes.values.toSeq.filterIt(it.id[^1] == 'A')
    var indices = currentNodes.mapIt(part1(path, nodes, it.id, x => x[^1] == 'Z'))
    
    return lcm(indices)


proc main() =
    let input = readInput(2023, 8, test = false).strip
    let map = parse(input)

    echo "Part 1: ", part1(map.path, map.nodes, "AAA", x => x == "ZZZ")
    echo "Part 2: ", part2(map.path, map.nodes)

main()