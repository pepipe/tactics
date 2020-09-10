using System.Linq;
using Tactics.Grid;
using Unity.Linq;
using UnityEngine;

namespace Tactics.Units
{
  public class UnitAiInput : IUnitInput
  {
    public bool AdquiredMovePosition { get; private set; }
    public bool CheckAttack { get; private set; }
    public Vector3 MovePosition { get; private set; }
    public Unit Target { get; private set; }

    private Unit m_Unit;
    private Unit m_TargetUnit;

    public UnitAiInput(Unit unit)
    {
      m_Unit = unit;
      Target = null;
    }

    public void MoveInput(SubGrid subGrid)
    {
      AdquiredMovePosition = false;
      CheckAttack = false;

      //Replacing this by using the TurnSystem Unit list
      //Check Players in grid | for weak first if equal for near first
      //m_TargetUnit = m_Unit.transform.parent.gameObject.Descendants()
      //                  .Select(x => x.GetComponent<Unit>())
      //                  .Where(x => x != null && x.Settings.UseAI != m_Unit.Settings.UseAI)
      //                  .OrderBy(x => x.Health)
      //                  .GroupBy(x => x.Health)                                         
      //                  .FirstOrDefault()
      //                  .OrderBy(x => Vector3.Distance(x.transform.position, m_Unit.transform.position))
      //                  .FirstOrDefault();
      if (m_Unit.SceneController.TurnSystem.PlayerUnits.Count > 0)
      {
        m_TargetUnit = m_Unit.SceneController.TurnSystem.PlayerUnits
                              .OrderBy(x => x.Health)
                              .GroupBy(x => x.Health)
                              .FirstOrDefault()
                              .OrderBy(x => Vector3.Distance(x.transform.position, m_Unit.transform.position))
                              .FirstOrDefault();
      }

      //Debug.Log(enemy.name);
      //Move to position closer to that player. -> Return World position of that.
      var targetNode = m_TargetUnit != null ?
                        subGrid.SubGridNodes
                            .OrderBy(x => Vector3.Distance(x.transform.position, m_TargetUnit.transform.position))
                            .FirstOrDefault() :
                        null;

      if (targetNode != null)
      {
        AdquiredMovePosition = true;
        MovePosition = targetNode.transform.position;
      }
    }

    public void AttackInput(SubGrid subGrid)
    {
      Target = subGrid.IsValidPosition(m_TargetUnit.GridPos) ? m_TargetUnit : null;
      CheckAttack = true;
    }
  }
}