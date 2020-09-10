using System;
using Tactics.Commands;
using Tactics.Grid;

namespace Tactics.Units.States
{
  public class ActiveState : BaseState
  {
    private Unit m_Unit;
    private MovementGrid m_MovementGrid;
    private bool m_GotPossibleMoves = false;

    public ActiveState(Unit unit) : base(unit.gameObject)
    {
      m_Unit = unit;
    }

    public override Type Tick()
    {
      //Debug.Log(m_Unit.name + " is in ActiveState");
      if (!m_Unit.IsActive)
      {
        DestroyMovementNodes();
        return typeof(IdleState);
      }else if (!m_GotPossibleMoves)
      {
        m_MovementGrid = new MovementGrid(m_Unit.SceneController.Grid, m_Unit.SceneController.MovementNodePrefab);
        m_MovementGrid.CreateSubGridNodes(m_Unit.Settings.MoveUnits, m_Unit.transform.position);
        m_GotPossibleMoves = true;
      }

      if (m_Unit.IsActive)
      {
        m_Unit.UnitInput.MoveInput(m_MovementGrid);
        if (m_Unit.UnitInput.AdquiredMovePosition)
        {
          var moveCommand = new MoveUnitCommand(m_Unit, m_Unit.UnitInput.MovePosition);
          m_Unit.SceneController.CommandProcessor.AddCommand(moveCommand);
          DestroyMovementNodes();
          return typeof(MovingState);
        }
      }
      return typeof(ActiveState);
    }

    private void DestroyMovementNodes()
    {
      m_GotPossibleMoves = false;
      m_MovementGrid.DestroyNodes();
    }
  }
}