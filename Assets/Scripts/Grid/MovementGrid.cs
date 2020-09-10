using UnityEngine;

namespace Tactics.Grid
{
  public class MovementGrid : SubGrid
  {
    public MovementGrid(Grid<GridObject> grid, GameObject nodeVisual) : base(grid, nodeVisual)
    {
    }

    public override void CreateSubGridNodes(int distanceFromPosition, Vector3 worldPosition)
    {
      var unitGridPos = m_Grid.GetXY(worldPosition);
      for (int x = -distanceFromPosition; x <= distanceFromPosition; ++x)
      {
        //nodes in same row of unit (same y)
        if (m_Grid.IsValidGridPosition(x + unitGridPos.x, unitGridPos.y) &&
              m_Grid.GetGridObject(x + unitGridPos.x, unitGridPos.y).Node.IsWalkable)
        {
          m_SubGridNodes.Add((x + unitGridPos.x, unitGridPos.y),
            CreateGridSelectionNode(x + unitGridPos.x, unitGridPos.y));
        }
        //nodes in differents row
        var rowDifference = distanceFromPosition - Mathf.Abs(x);
        for (int y = -rowDifference; y <= rowDifference; ++y)
        {
          if (y != 0 && m_Grid.IsValidGridPosition(x + unitGridPos.x, y + unitGridPos.y) &&
                m_Grid.GetGridObject(x + unitGridPos.x, y + unitGridPos.y).Node.IsWalkable)
          {
            m_SubGridNodes.Add((x + unitGridPos.x, y + unitGridPos.y),
              CreateGridSelectionNode(x + unitGridPos.x, y + unitGridPos.y));
          }
        }
      }
    }
  }
}
