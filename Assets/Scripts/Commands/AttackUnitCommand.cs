using Tactics.Units;

namespace Tactics.Commands
{
  public class AttackUnitCommand : ICommand
  {
    private Unit m_Unit;
    private Unit m_Target;

    public AttackUnitCommand(Unit unit, Unit target)
    {
      m_Unit = unit;
      m_Target = target;
    }

    public void Execute()
    {
      m_Target.TakeDamage(m_Unit.Settings.AttackDamage);
      m_Unit.SceneController.TriggerOnCallACommand(m_Unit.name + " attacked " 
                        + m_Target.name + " with " + m_Unit.Settings.AttackDamage + " damage");
    }
  }
}