using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    public const float CellHeight = 2;
    public const float CellWidth = 2;

    public GameObject Sprite;

    public Tilemap Walls;
    public static GridSystem Instance;

    void Awake()
    {
        Instance = this;
    }

    public static Vector2 SnapWorldPosition(Vector3 worldPos)
    {
        var cell = new Vector2Int(Mathf.FloorToInt(worldPos.x / CellWidth), Mathf.FloorToInt(worldPos.y / CellHeight));
        return new Vector2(CellWidth * cell.x, CellHeight * cell.y) + new Vector2(CellWidth, CellHeight) * 0.5f;
    }

    void Update()
    {
        var position = SnapWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Sprite.transform.position = new Vector3(position.x, position.y, 1);
    }

    public PathNode[] FindPathTo(Vector3 from, Vector3 to)
    {
        var pQueue = new PathQueue();
        var visited = new List<Path>();
        int iterations = 0;
        pQueue.Push(new Path(new PathNode(from, to, 0)));
        do
        {
            var path = pQueue.Pop();
            if (path.PathNode.cumulative > 20)
            {
                continue;
            }
            if (path.PathNode.x == to)
            {
                //Debug.Log(iterations);
                return BuildPath(path);
            }
            //North
            var nx = path.PathNode.x + new Vector3(0, CellHeight, 0);
            CheckNext(path, nx, visited, pQueue, to);

            //South
            nx = path.PathNode.x + new Vector3(0, -CellHeight, 0);
            CheckNext(path, nx, visited, pQueue, to);

            //East
            nx = path.PathNode.x + new Vector3(CellWidth, 0, 0);
            CheckNext(path, nx, visited, pQueue, to);

            //West 
            nx = path.PathNode.x + new Vector3(-CellWidth, 0, 0);
            CheckNext(path, nx, visited, pQueue, to);
            iterations++;
        } while (pQueue.Count > 0);
        return null;
    }

    private static PathNode[] BuildPath(Path path)
    {
        var finalPath = new List<PathNode>();
        do
        {
            finalPath.Add(path.PathNode);
            path = path.Previous;
        } while (path != null);
        finalPath.Reverse();
        return finalPath.ToArray();
    }

    private void CheckNext(Path prev, Vector3 nx, List<Path> visitedPaths, PathQueue pQueue,Vector3 target)
    {
        var current = new Path(prev, new PathNode(nx, target, prev.PathNode.cumulative + (CheckCell(nx) ? int.MaxValue : 1)));
        var isVisited = false;
        for (var i = 0; i < visitedPaths.Count; i++)
        {
            var pathNode = visitedPaths[i];
            if (pathNode.PathNode.x == current.PathNode.x)
            {
                isVisited = true;
                if (pathNode.PathNode.fx > current.PathNode.fx)
                {
                    visitedPaths[i] = current;
                    pQueue.Push(current);
                }
            }
        }
        if (!isVisited)
        {
            visitedPaths.Add(current);
            pQueue.Push(current);
        }
    }

    public bool CheckCell(Vector3 vector3)
    {
        if (Walls == null) return false;
        return Walls.HasTile(Walls.layoutGrid.WorldToCell(vector3));
    }

    public void SetColor(Color color)
    {
        Sprite.GetComponent<SpriteRenderer>().color = color;
    }
}

public class PathQueue
{
    public List<Path> P;
    public PathQueue()
    {
        P = new List<Path>();
    }

    public int Count => P.Count;

    public void Push(Path path)
    {
        if (P.Count < 1)
        {
            P.Add(path);
            return;
        }

        var existingPath = P.FirstOrDefault(m => m.PathNode.x == path.PathNode.x);
        if (existingPath != null)
        {
            P.RemoveAt(P.IndexOf(existingPath));
        }
        //for (int i = 0; i < P.Count; i++)
        //{
        //    if (path.PathNode.fx <= P[i].PathNode.fx)
        //    {
        //        P.Insert(i, path);
        //        return;
        //    }
        //}
        var index = P.FindIndex(m => m.PathNode.fx > path.PathNode.fx);
        if (index >= 0)
            P.Insert(index, path);
        else
            P.Add(path);

    }

    public Path Pop()
    {
        var node = P[0];
        P.RemoveAt(0);
        return node;
    }

}

public class Path
{
    public Path Previous;
    public PathNode PathNode;

    public Path(PathNode pathNode)
    {
        PathNode = pathNode;
    }
    public Path(Path previous, PathNode pathNode)
    {
        Previous = previous;
        PathNode = pathNode;
    }
}

public struct PathNode
{
    public Vector3 x;
    public float fx => cumulative + estimate;
    public float cumulative;
    private float estimate;

    public PathNode(Vector3 x, Vector3 target, float cum)
    {
        this.x = x;
        cumulative = cum;
        estimate = (target - x).sqrMagnitude;
    }
}
