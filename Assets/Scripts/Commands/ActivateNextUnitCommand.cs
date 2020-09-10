namespace Tactics.Commands
{
  public class ActivateNextUnitCommand : ICommand
  {
    private SceneController m_SceneController;
    private TurnSystem m_TurnSystem;

    private const string COMMAND_TEXT = "Activate Unit: ";

    public ActivateNextUnitCommand(SceneController sceneController, TurnSystem turnSystem)
    {
      m_SceneController = sceneController;
      m_TurnSystem = turnSystem;
    }

    public void Execute()
    {
      var unit = m_TurnSystem.ActivateNextUnit();
      if (unit != null)
      {
        m_SceneController.SetActiveUnit(unit);
        m_SceneController.TriggerOnCallACommand(COMMAND_TEXT + unit.name);
      }
      else if(m_SceneController.TurnSystem.GetActivePlayerUnitsNumber() > 0)
        m_SceneController.EndTurn();
    }
  }
}