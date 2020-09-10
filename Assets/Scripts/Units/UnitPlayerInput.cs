using System.Linq;
using Tactics.Grid;
using Unity.Linq;
using UnityEngine;

namespace Tactics.Units
{
  public class UnitPlayerInput : IUnitInput
  {
    public bool AdquiredMovePosition { get; private set; }
    public bool CheckAttack { get; private set; }
    public Vector3 MovePosition { get; private set; }
    public Unit Target { get; private set; }

    private Unit m_Unit;
    public UnitPlayerInput(Unit unit)
    {
      m_Unit = unit;
      Target = null;
    }

    public void MoveInput(SubGrid subGrid)
    {
      CheckAttack = false;
      AdquiredMovePosition = false;
      if (Input.GetMouseButtonDown(0))
      {
        Vector3 mouseWorldPosition = Utils.GetMouseWorldPosition(Camera.main);
        if (m_Unit.SceneController.Grid.IsValidGridPosition(mouseWorldPosition) &&
            subGrid.IsValidPosition(m_Unit.SceneController.Grid.GetXY(mouseWorldPosition)))
        {
          MovePosition = mouseWorldPosition;
          AdquiredMovePosition = true;
        }
      }
    }

    public void AttackInput(SubGrid subGrid)
    {
      Target = null;
      if (Input.GetMouseButtonDown(0))
      {
        Vector3 mouseWorldPosition = Utils.GetMouseWorldPosition(Camera.main);
        var attackPos = m_Unit.SceneController.Grid.GetXY(mouseWorldPosition);
        if (m_Unit.SceneController.Grid.IsValidGridPosition(mouseWorldPosition) &&
            subGrid.IsValidPosition(attackPos))
        {
          //Replacing this by using the TurnSystem Unit list
          //Target = m_Unit.transform.parent.gameObject.Descendants()
          //              .Select(x => x.GetComponent<Unit>())
          //              .Where(x => x != null && x.Settings.UseAI != m_Unit.Settings.UseAI)
          //              .Where(x => x.GetComponent<Unit>().GridPos.X == attackPos.x &&
          //                            x.GetComponent<Unit>().GridPos.Y == attackPos.y)
          //              .FirstOrDefault();
          Target = m_Unit.SceneController.TurnSystem.AIUnits
                    .Where(x => x.GetComponent<Unit>().GridPos.X == attackPos.x &&
                                  x.GetComponent<Unit>().GridPos.Y == attackPos.y)
                    .FirstOrDefault();

          if (Target != null && m_Unit.SceneController.Grid.GetXY(mouseWorldPosition) == Target.GridPos)
          {
            CheckAttack = true;
          }
        }
      }
    }
  }
}