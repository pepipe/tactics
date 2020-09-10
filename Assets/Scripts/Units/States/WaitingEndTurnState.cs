using System;
using UnityEngine;

namespace Tactics.Units.States
{
  public class WaitingEndTurnState : BaseState
  {
    private Unit m_Unit;

    public WaitingEndTurnState(Unit unit) : base(unit.gameObject)
    {
      m_Unit = unit;
    }

    public override Type Tick()
    {
      //Debug.Log(m_Unit.name + " is in WaitingEndTurnState");
      if (m_Unit.AdvanceState)//reset turn
      {
        m_Unit.AdvanceState = false;
        return typeof(IdleState);
      }

      return typeof(WaitingEndTurnState);
    }
  }
}