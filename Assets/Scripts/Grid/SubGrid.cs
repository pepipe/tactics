using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Grid
{
  public abstract class SubGrid
  {
    protected Grid<GridObject> m_Grid;
    protected GameObject m_NodeVisual;

    public IEnumerable<GameObject> SubGridNodes { get => m_SubGridNodes.Values; }
    protected Dictionary<(int x, int y), GameObject> m_SubGridNodes = new Dictionary<(int x, int y), GameObject>();

    protected SubGrid(Grid<GridObject> grid, GameObject nodeVisual)
    {
      m_Grid = grid;
      m_NodeVisual = nodeVisual;
    }

    public abstract void CreateSubGridNodes(int distanceFromPosition, Vector3 worldPosition);

    public virtual bool IsValidPosition((int x, int y) gridPos)
    {
      return m_SubGridNodes.ContainsKey(gridPos);
    }

    public virtual void DestroyNodes()
    {
      foreach (var node in m_SubGridNodes)
      {
        GameObject.Destroy(node.Value);
      }
      m_SubGridNodes.Clear();
    }

    protected virtual GameObject CreateGridSelectionNode(int gridX, int gridY)
    {
      var worldPosition = m_Grid.GetWorldPosition(gridX, gridY);
      var gridObj = m_Grid.GetGridObject(worldPosition);
      var movementNodeVisual = GameObject.Instantiate(m_NodeVisual,
                                  gridObj.transform,
                                  true);
      movementNodeVisual.transform.position = gridObj.transform.position;
      return movementNodeVisual;
    }
  }
}