using System;

namespace Tactics.Units.States
{
  public class MovingState : BaseState
  {
    private Unit m_Unit;

    public MovingState(Unit unit) : base(unit.gameObject)
    {
      m_Unit = unit;
    }

    public override Type Tick()
    {
      //Debug.Log(m_Unit.name + " is in MovingState");
      if (m_Unit.Path != null)
      {        
        m_Unit.Move();
        return typeof(MovingState);
      }

      return typeof(AttackState);
    }
  }
}