include ../prelude
import std/re, sets, sequtils, "../ds/graph", algorithm
import "../utils/seq"

func parseIntoGraph(lines: seq[string]): Graph[int] =
    var graph: Graph[int] = initGraph[int]()
    var valveRegex = re"^Valve (?<valve>\w+) has flow rate=(?<flow>\d+); tunnels? leads? to valves? (?<valves>(?:\w+,?\s?)+)$"

    for line in lines:
        var matches: array[3, string]
        assert match(line, valveRegex, matches), line

        graph[matches[0]] = GraphNode[int](
            id: matches[0], 
            value: matches[1].parseInt, 
            edges: matches[2].split(", "))

    return graph

func filterOutNoFlowValves(valves: Graph[int]): seq[(string, int)] =
    let valvesWithFlow = collect:
        for valve in valves.values:
            if valve.value != 0:
                (valve.id, valve.value)

    return valvesWithFlow.sortedByIt(it[1]).reversed

func calculateRoute(shortestPaths: ShortestPathLookup[int], path: seq[string]): seq[string] =
    var steps = path

    var curr = steps[0]
    steps = steps.filterIt(it != curr)

    result = @[curr]

    while steps.len > 1:
        var next = steps[0]
        var p = shortestPaths[(curr, next)].mapIt(it.id)

        result.add(p)

        steps = steps.filterIt(it != next)
        curr = next   

proc simulateFlow(valves: Graph[int], shortestPaths: ShortestPathLookup[int], maxTime: int, path: seq[string]): int =
    result = 0
    var open = initHashSet[string]()
    var remainingTime = maxTime

    for valve in path:
        if not open.contains(valve):
            open.incl(valve)

            let valveNode = valves[valve]
            let flow = valveNode.value

            if flow != 0:
                let totalFlow = flow * remainingTime

                echo "Opening ", valve, " for ", remainingTime, " minutes, flow rate ", flow, " total flow ", totalFlow
                result += totalFlow
                remainingTime -= 1
        remainingTime -= 1



let lines = readInput(2022, 16, test=true).strip.splitLines
let valves = parseIntoGraph(lines)

echo "Calculating shortest paths..."
let shortestPaths = valves.floyd_warshal

var valvesWithFlow = filterOutNoFlowValves(valves).mapIt(it[0])
let possiblePaths = valvesWithFlow
    .permutations
    .mapIt(concat(@["AA"], it)) # Add the start to each
    .deduplicate
    .sortedByIt(it.len)

for p in possiblePaths:
    echo p


