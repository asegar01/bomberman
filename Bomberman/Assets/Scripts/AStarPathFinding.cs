using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder
{
    private Grid grid;

    public AStarPathfinder(Grid grid)
    {
        this.grid = grid;
    }

    public List<Vector3Int> FindPath(Vector3Int startPos, Vector3Int targetPos)
    {
        Node startNode = new Node(startPos);
        Node targetNode = new Node(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighborNode in GetNeighborNodes(currentNode))
            {
                if (closedSet.Contains(neighborNode))
                {
                    continue;
                }

                int movementCost = currentNode.GCost + GetDistance(currentNode, neighborNode);
                if (movementCost < neighborNode.GCost || !openSet.Contains(neighborNode))
                {
                    neighborNode.GCost = movementCost;
                    neighborNode.HCost = GetDistance(neighborNode, targetNode);
                    neighborNode.Parent = currentNode;

                    if (!openSet.Contains(neighborNode))
                    {
                        openSet.Add(neighborNode);
                    }
                }
            }
        }

        return null;
    }

    private List<Node> GetNeighborNodes(Node node)
    {
        List<Node> neighborNodes = new List<Node>();

        Vector3Int[] directions = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.right,
            Vector3Int.down,
            Vector3Int.left
        };

        foreach (Vector3Int direction in directions)
        {
            Vector3Int neighborPos = node.Position + direction;
            //if ()
            //{
            //    neighborNodes.Add(new Node(neighborPos));
            //}
        }

        return neighborNodes;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
        int distanceY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

        return distanceX + distanceY;
    }

    private List<Vector3Int> RetracePath(Node startNode, Node endNode)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }

    private class Node
    {
        public Vector3Int Position;
        public int GCost;
        public int HCost;
        public Node Parent;

        public int FCost { get { return GCost + HCost; } }

        public Node(Vector3Int position)
        {
            Position = position;
        }
    }
}