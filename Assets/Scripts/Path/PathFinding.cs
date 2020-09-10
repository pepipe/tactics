using System.Collections.Generic;
using Tactics.Grid;
using UnityEngine;

namespace Tactics.Paths
{
  public class PathFinding
  {
    //Moving horizontally is straight so it has a simple cost of 1, 
    //moving diagonally has a cost of square root of 2 which is 1.4.
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private Grid<GridObject> m_Grid;
    private List<PathNode> m_OpenList;
    private List<PathNode> m_ClosedList;

    public PathFinding(Grid<GridObject> grid)
    {
      m_Grid = grid;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
      var (startX, startY) = m_Grid.GetXY(startWorldPosition);
      var (endX, endY) = m_Grid.GetXY(endWorldPosition);

      List<PathNode> path = FindPath(startX, startY, endX, endY);
      if (path != null)
      {
        List<Vector3> vectorPath = new List<Vector3>();
        foreach (PathNode pathNode in path)
          vectorPath.Add(new Vector3(pathNode.XPos, pathNode.YPos) * m_Grid.GridCellSize + Vector3.one * m_Grid.GridCellSize * .5f);

        return vectorPath;
      }

      return null;
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
      PathNode startNode = m_Grid.GetGridObject(startX, startY)?.Node;
      PathNode endNode = m_Grid.GetGridObject(endX, endY)?.Node;

      if (startNode == null || endNode == null || !endNode.IsWalkable)
      {
        // Invalid Path
        return null;
      }

      m_OpenList = new List<PathNode> { startNode };
      m_ClosedList = new List<PathNode>();

      for (int x = 0; x < m_Grid.GridWidth; x++)
      {
        for (int y = 0; y < m_Grid.GridHeight; y++)
        {
          PathNode pathNode = m_Grid.GetGridObject(x, y).Node;
          pathNode.GCost = 99999999;
          pathNode.CalculateFCost();
          pathNode.CameFromNode = null;
        }
      }

      startNode.GCost = 0;
      startNode.HCost = CalculateDistanceCost(startNode, endNode);
      startNode.CalculateFCost();

      while (m_OpenList.Count > 0)
      {
        PathNode currentNode = GetLowestFCostNode(m_OpenList);
        if (currentNode == endNode)
        {
          return CalculatePath(endNode);
        }

        m_OpenList.Remove(currentNode);
        m_ClosedList.Add(currentNode);

        foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
        {
          if (m_ClosedList.Contains(neighbourNode)) 
            continue;

          if (!neighbourNode.IsWalkable)
          {
            m_ClosedList.Add(neighbourNode);
            continue;
          }

          int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbourNode);
          if (tentativeGCost < neighbourNode.GCost)
          {
            neighbourNode.CameFromNode = currentNode;
            neighbourNode.GCost = tentativeGCost;
            neighbourNode.HCost = CalculateDistanceCost(neighbourNode, endNode);
            neighbourNode.CalculateFCost();

            if (!m_OpenList.Contains(neighbourNode))
            {
              m_OpenList.Add(neighbourNode);
            }
          }
        }
      }

      // Out of nodes on the openList
      return null;
    }

    public List<Vector3> GetVector3Path(List<PathNode> path)
    {
      if (path != null)
      {
        List<Vector3> vectorPath = new List<Vector3>();
        foreach (PathNode pathNode in path)
        {
          vectorPath.Add(new Vector3(pathNode.XPos, pathNode.YPos) * m_Grid.GridCellSize + Vector3.one * m_Grid.GridCellSize * .5f);
        }
        return vectorPath;
      }

      return null;
    }

    public void PrintPath(List<PathNode> path)
    {
      if (path == null)
      {
        Debug.Log("Path empty");
        return;
      }
      Debug.Log("===Start Path===");

      foreach (var node in path)
        Debug.Log(node.ToString());

      Debug.Log("===End Path===");
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
      List<PathNode> neighbourList = new List<PathNode>();

      if (currentNode.XPos - 1 >= 0)
      {
        // Left
        var node = GetNode(currentNode.XPos - 1, currentNode.YPos);
        if(node != null)
          neighbourList.Add(node);
        // Left Down
        if (currentNode.YPos - 1 >= 0)
        {
          var n = GetNode(currentNode.XPos - 1, currentNode.YPos - 1);
          if (n != null)
            neighbourList.Add(n);
        }
        // Left Up
        if (currentNode.YPos + 1 < m_Grid.GridWidth)
        {
          var n = GetNode(currentNode.XPos - 1, currentNode.YPos + 1);
          if (n != null)
            neighbourList.Add(n);
        }
      }
      if (currentNode.XPos + 1 < m_Grid.GridWidth)
      {
        // Right
        var node = GetNode(currentNode.XPos + 1, currentNode.YPos);
        if (node != null)
          neighbourList.Add(node);
        // Right Down
        if (currentNode.YPos - 1 >= 0)
        {
          var n = GetNode(currentNode.XPos + 1, currentNode.YPos - 1);
          if (n != null)
            neighbourList.Add(n);
        }
        // Right Up
        if (currentNode.YPos + 1 < m_Grid.GridHeight)
        {
          var n = GetNode(currentNode.XPos + 1, currentNode.YPos + 1);
          if (n != null)
            neighbourList.Add(n);
        }
      }
      // Down
      if (currentNode.YPos - 1 >= 0)
      {
        var n = GetNode(currentNode.XPos, currentNode.YPos - 1);
        if (n != null)
          neighbourList.Add(n);
      }
      // Up
      if (currentNode.YPos + 1 < m_Grid.GridHeight)
      {
        var n = GetNode(currentNode.XPos, currentNode.YPos + 1);
        if (n != null)
          neighbourList.Add(n);
      }

      return neighbourList;
    }

    private PathNode GetNode(int x, int y)
    {
      return m_Grid.GetGridObject(x, y)?.Node;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
      List<PathNode> path = new List<PathNode>();
      path.Add(endNode);
      PathNode currentNode = endNode;
      while (currentNode.CameFromNode != null)
      {
        path.Add(currentNode.CameFromNode);
        currentNode = currentNode.CameFromNode;
      }
      path.Reverse();
      return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
      int xDistance = Mathf.Abs(a.XPos - b.XPos);
      int yDistance = Mathf.Abs(a.YPos - b.YPos);
      int remaining = Mathf.Abs(xDistance - yDistance);
      return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
      PathNode lowestFCostNode = pathNodeList[0];
      for (int i = 1; i < pathNodeList.Count; i++)
      {
        if (pathNodeList[i].FCost < lowestFCostNode.FCost)
        {
          lowestFCostNode = pathNodeList[i];
        }
      }
      return lowestFCostNode;
    }
  }
}