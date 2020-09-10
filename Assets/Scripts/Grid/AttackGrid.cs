using UnityEngine;

namespace Tactics.Grid
{
  public class AttackGrid : SubGrid
  {
    public AttackGrid(Grid<GridObject> grid, GameObject nodeVisual) : base(grid, nodeVisual)
    {
    }

    public override void CreateSubGridNodes(int distanceFromPosition, Vector3 worldPosition)
    {
      var unitGridPos = m_Grid.GetXY(worldPosition);
      if (distanceFromPosition > 0)
        CreateAttackGrid(distanceFromPosition, unitGridPos);
      else
        CreateRangeZeroGrid(unitGridPos);
    }

    private void CreateRangeZeroGrid((int x, int y) unitGridPos)
    {
      for (int x = -1; x <= 1; ++x)
        for (int y = -1; y <= 1; ++y)
        {
          if (m_Grid.IsValidGridPosition(x + unitGridPos.x, y + unitGridPos.y))
          {
            m_SubGridNodes.Add((x + unitGridPos.x, y + unitGridPos.y),
              CreateGridSelectionNode(x + unitGridPos.x, y + unitGridPos.y));
          }
        }
    }

    private void CreateAttackGrid(int distanceFromPosition, (int x, int y) unitGridPos)
    {
      for (int x = -distanceFromPosition; x <= distanceFromPosition; ++x)
      {
        //nodes in same row of unit (same y)
        if (m_Grid.IsValidGridPosition(x + unitGridPos.x, unitGridPos.y))
        {
          m_SubGridNodes.Add((x + unitGridPos.x, unitGridPos.y),
            CreateGridSelectionNode(x + unitGridPos.x, unitGridPos.y));
        }
        //nodes in differents row
        var rowDifference = distanceFromPosition - Mathf.Abs(x);
        for (int y = -rowDifference; y <= rowDifference; ++y)
        {
          if (y != 0 && m_Grid.IsValidGridPosition(x + unitGridPos.x, y + unitGridPos.y))
          {
            m_SubGridNodes.Add((x + unitGridPos.x, y + unitGridPos.y),
              CreateGridSelectionNode(x + unitGridPos.x, y + unitGridPos.y));
          }
        }
      }
    }
  }
}
