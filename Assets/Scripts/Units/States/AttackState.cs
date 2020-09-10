using System;
using Tactics.Grid;
using Tactics.Commands;
using UnityEngine;

namespace Tactics.Units.States
{
  public class AttackState : BaseState
  {
    private Unit m_Unit;
    private AttackGrid m_AttackGrid;
    private bool m_GotPossibleAttacks = false;

    public AttackState(Unit unit) : base(unit.gameObject)
    {
      m_Unit = unit;
    }

    public override Type Tick()
    {
      //Debug.Log(m_Unit.name + " is in AttackState");
      if ((!m_Unit.IsActive && m_AttackGrid != null))
      {
        DestroyAttackNodes();
        return typeof(WaitingEndTurnState);
      }
      else if (!m_GotPossibleAttacks)
      {
        m_AttackGrid = new AttackGrid(m_Unit.SceneController.Grid, m_Unit.SceneController.AttackNodePrefab);
        m_AttackGrid.CreateSubGridNodes(m_Unit.Settings.AttackRange, m_Unit.transform.position);
        m_GotPossibleAttacks = true;
      }

      if (m_Unit.IsActive)
      {
        m_Unit.UnitInput.AttackInput(m_AttackGrid);

        if (m_Unit.UnitInput.Target != null)
        {
          var attackCommand = new AttackUnitCommand(m_Unit, m_Unit.UnitInput.Target);
          m_Unit.SceneController.CommandProcessor.AddCommand(attackCommand);
        }

        if (m_Unit.UnitInput.CheckAttack)
        {
          var nextUnitCommand = new ActivateNextUnitCommand(m_Unit.SceneController, m_Unit.SceneController.TurnSystem);
          m_Unit.SceneController.CommandProcessor.AddCommand(nextUnitCommand);
          DestroyAttackNodes();
          return typeof(WaitingEndTurnState);
        }
      }

      return typeof(AttackState);
    }

    private void DestroyAttackNodes()
    {
      m_GotPossibleAttacks = false;
      m_AttackGrid.DestroyNodes();
      m_AttackGrid = null;
    }
  }
}