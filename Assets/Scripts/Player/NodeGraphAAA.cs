using UnityEngine;
using System.Collections.Generic;
public class NodeGraphAAA : MonoBehaviour
{
    [Header("All registered nodes in the scene")]
    public List<NodeRoadAAA> allNodes;

    /// <summary>
    /// Finds the shortest path from start to goal using BFS.
    /// </summary>
    public List<NodeRoadAAA> FindPath(NodeRoadAAA start, NodeRoadAAA goal)
    {
        if (start == null || goal == null)
        {
            Debug.LogWarning("❌ Start or Goal node is null.");
            return null;
        }

        Queue<NodeRoadAAA> frontier = new Queue<NodeRoadAAA>();
        Dictionary<NodeRoadAAA, NodeRoadAAA> cameFrom = new Dictionary<NodeRoadAAA, NodeRoadAAA>();

        frontier.Enqueue(start);
        cameFrom[start] = null;

        while (frontier.Count > 0)
        {
            NodeRoadAAA current = frontier.Dequeue();

            if (current == goal)
                break;

            foreach (NodeRoadAAA neighbor in current.connectedNodes)
            {
                if (neighbor != null && !cameFrom.ContainsKey(neighbor))
                {
                    frontier.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        if (!cameFrom.ContainsKey(goal))
        {
            Debug.LogWarning("❌ No path to goal found.");
            return null;
        }

        List<NodeRoadAAA> path = new List<NodeRoadAAA>();
        NodeRoadAAA step = goal;

        while (step != null)
        {
            path.Add(step);
            step = cameFrom[step];
        }

        path.Reverse(); // Make it go from start → goal
        return path;
    }
}