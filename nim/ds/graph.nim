import strformat
import tables


type GraphNode*[T] = object
    id*: string
    value*: T
    edges*: seq[string]

type Graph*[T] = Table[string, GraphNode[T]]

type ShortestPathLookup*[T] = Table[tuple[start: string, dest: string], seq[GraphNode[T]]]

proc `$`*[T](node: GraphNode[T]): string =
    let edges = node.edges.join(", ")
    result = fmt"{node.id} ({node.value}) > {edges}"

proc initGraph*[T](): Graph[T] =
    result = initTable[string, GraphNode[T]]()

proc floyd_warshal*[T](graph: Graph[T]): ShortestPathLookup[T] =
    ## Floyd-Warshal algorithm for finding the shortest path between all nodes in a graph
    ## https://www.wikiwand.com/en/Floyd%E2%80%93Warshall_algorithm

    # Key is (from, to) - value is steps in shortest path
    result = initTable[(string, string), seq[GraphNode[T]]]()
    
    var veryLongList: seq[GraphNode[T]] = @[]
    veryLongList.setLen(100)

    # Initialize the table with the direct edges
    for node in graph.values:
        for edge in node.edges:
            result[(node.id, edge)] = @[graph[edge]]

    # For each node, check if there is a shorter path through another node
    for k in graph.values:
        for i in graph.values:
            for j in graph.values:
                let pathIJ = result.getOrDefault((i.id, j.id), veryLongList)
                
                let pathIK = result.getOrDefault((i.id, k.id), veryLongList)
                let pathKJ = result.getOrDefault((k.id, j.id), veryLongList)
                let pathIJviaK = pathIK & pathKJ

                if pathIJviaK.len < pathIJ.len:
                    result[(i.id, j.id)] = pathIJviaK


            