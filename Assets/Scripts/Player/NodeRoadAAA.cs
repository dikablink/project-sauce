using UnityEngine;
using System.Collections.Generic;
public class NodeRoadAAA : MonoBehaviour
{
    [Header("Connected Nodes")]
    public List<NodeRoadAAA> connectedNodes = new List<NodeRoadAAA>();
 public NodeRoadAAA nextNode;
    [Header("Debug")]
    public bool showConnections = true;

    private void OnDrawGizmos()
    {
        if (!showConnections) return;

        Gizmos.color = Color.green;
        foreach (var node in connectedNodes)
        {
            if (node != null)
                Gizmos.DrawLine(transform.position, node.transform.position);
        }
        if (nextNode != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, nextNode.transform.position);
        }
    }
}