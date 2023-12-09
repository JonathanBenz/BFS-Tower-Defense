using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }

    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DesinationCoordinates { get { return destinationCoordinates; } }

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    // These two data containers will be useful for tracking what the next nodes to explore will be and which ones we have already visited.
    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null) grid = gridManager.Grid;

        startNode = grid[startCoordinates];
        destinationNode = grid[destinationCoordinates];
    }
    // Start is called before the first frame update
    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }
    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNodes();
        BFS(coordinates);
        return BuildPath();
    }
    void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();
        foreach(Vector2Int direction in directions)
        {
            Vector2Int neighborCoordinate = currentSearchNode.coordinates + direction;
            //Node neighbor = gridManager.GetNode(neighborCoordinate);
            if (grid.ContainsKey(neighborCoordinate))
            //if(neighbor != null)
            {
                neighbors.Add(grid[neighborCoordinate]);
                //neighbors.Add(neighbor);
            }
        }
        foreach(Node n in neighbors)
        {
            // if we have not already reached this node, and it is walkable, then we can place this in our frontier.
            if (!reached.ContainsKey(n.coordinates) && n.isWalkable)
            {
                n.connectedTo = currentSearchNode;
                reached.Add(n.coordinates, n);
                frontier.Enqueue(n);
            }
        }
    }

    void BFS(Vector2Int startingCoordinate)
        /* startingCoordinate tells our BFS to start wherever we tell it to. This way, if the enemies
           have to recalculate their path in the middle of the map, then the startingCoordinate will be their
           current position and not the very beginning of the level. */
    {
        // These are the only nodes where isWalkable is true but isPlacable is false.
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear();
        reached.Clear();

        bool isRunning = true;

        frontier.Enqueue(grid[startingCoordinate]);
        reached.Add(startingCoordinate, grid[startingCoordinate]);

        while(frontier.Count > 0 && isRunning)
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if (currentSearchNode.coordinates == destinationCoordinates) isRunning = false;
        }
    }

    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while(currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();

        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates)
    {
        if(grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable;
            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            // Since BuildPath() works backwards, if there are no connected nodes to the destination node (e.g. the count is not more than 1) then you know there is no possible path (or in other words the path is blocked). 
            if(newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }
        // Else a new path was possible to be found, so the path was not blocked! 
        return false;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
