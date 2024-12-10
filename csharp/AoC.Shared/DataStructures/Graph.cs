using System.Text;
using AoC.Shared.Utils;
using static AoC.Shared.DataStructures.CharGrid2D;

namespace AoC.Shared.DataStructures;

public class Graph<T>
{
    public List<GraphNode<T>> Nodes { get; } = new();

    public static Graph<Cell> From(CharGrid2D grid, Func<char, bool>? filter = null)
    {
        var graph = new Graph<Cell>();

        // Add Every Node
        grid.ForEach(c =>
        {
            if (filter != null && !filter(c.Char)) return;
            graph.Nodes.Add(GraphNode<Cell>.Empty(c));
        });

        // Add Every Edge
        foreach (var node in graph.Nodes)
        {
            foreach (var neighborCell in node.Value.Neighbors(Direction2D.CardinalDirections, grid))
            {
                if (filter != null && !filter(neighborCell.Char)) continue;

                var neighborNode = graph.Nodes.Single(n => n.Value == neighborCell);

                node.Neighbors.Add(neighborNode);
            }
        }

        return graph;
    }

    public void PruneNeighbors(Func<GraphNode<T>, GraphNode<T>, bool> predicate,
        Action<GraphNode<T>, GraphNode<T>>? onPrune = null)
    {
        foreach (var node in Nodes)
        {
            node.Neighbors.RemoveAll(neighbor =>
            {
                if (!predicate(node, neighbor)) return false;

                onPrune?.Invoke(node, neighbor);

                return true;
            });
        }
    }

    /// <summary>
    /// BFS to find the shortest path between two nodes
    /// Assumes that the graph is unweighted
    /// Returns null if no path is found
    /// </summary>
    public List<GraphNode<T>>? ShortestPathBfs(
        GraphNode<T> start,
        GraphNode<T> end,
        Func<GraphNode<T>, bool> skipCondition = null)
    {
        // Copilot generated code - idk if it works
        var queue = new Queue<GraphNode<T>>();
        var visited = new HashSet<GraphNode<T>>();
        var parentMap = new Dictionary<GraphNode<T>, GraphNode<T>>();

        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (!visited.Add(current)) continue;

            if (current == end)
            {
                var path = new List<GraphNode<T>>();
                var node = end;

                while (node != start)
                {
                    path.Add(node);
                    node = parentMap[node];
                }

                path.Add(start);
                path.Reverse();

                return path;
            }

            foreach (var neighbor in current.Neighbors)
            {
                if (visited.Contains(neighbor)) continue;
                if (neighbor != end && skipCondition?.Invoke(neighbor) == true) continue;

                queue.Enqueue(neighbor);
                parentMap[neighbor] = current;
            }
        }

        return null;
    }

    public List<List<GraphNode<T>>> GetAllPathsDfs(
        GraphNode<T> start, 
        GraphNode<T> end, 
        Func<GraphNode<T>, bool> nodeSkipCondition = null,
        Func<List<GraphNode<T>>, bool> pathSkipCondition = null)
    {
        var allPaths = new List<List<GraphNode<T>>>();
        var visited = new HashSet<GraphNode<T>>();
        var currentPath = new List<GraphNode<T>>();
        DfsHelper(start);
        return allPaths;

        void DfsHelper(GraphNode<T> current)
        {
            visited.Add(current);
            currentPath.Add(current);

            if (current == end)
            {
                allPaths.Add([..currentPath]);
            }
            else
            {
                if (pathSkipCondition == null || !pathSkipCondition(currentPath))
                {
                    foreach (var neighbor in current.Neighbors)
                    {
                        if (visited.Contains(neighbor))
                            continue;

                        if (neighbor != end && nodeSkipCondition != null && nodeSkipCondition(neighbor))
                            continue;

                        DfsHelper(neighbor);
                    }
                }
            }

            // Backtrack
            visited.Remove(current);
            currentPath.RemoveAt(currentPath.Count - 1);
        }
    }

}

public sealed record GraphNode<T>(T Value, List<GraphNode<T>> Neighbors)
{
    public static GraphNode<T> Empty(T value) => new(value, new());

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append($"Node: {Value}");
        sb.Append("\nNeighbors:\n\t");
        sb.AppendJoin("\n\t", Neighbors.Select(n => n.Value));

        return sb.ToString();
    }
};