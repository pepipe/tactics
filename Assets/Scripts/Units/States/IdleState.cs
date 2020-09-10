using System;

namespace Tactics.Units.States
{
  public class IdleState : BaseState
  {
    private Unit m_Unit;

    public IdleState(Unit unit) : base(unit.gameObject)
    {
      m_Unit = unit;
    }

    public override Type Tick()
    {
      //Debug.Log(m_Unit.name + " is in IdleState");
      if (m_Unit.IsActive)
      {
        return typeof(ActiveState);
      }

      return null;
    }
  }
}