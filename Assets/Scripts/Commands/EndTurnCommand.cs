namespace Tactics.Commands
{
  public class EndTurnCommand : ICommand
  {
    private SceneController m_SceneController;
    private TurnSystem m_TurnSystem;

    private const string COMMAND_TEXT = "EndTurn of ";

    public EndTurnCommand(SceneController sceneController, TurnSystem turnSystem)
    {
      m_SceneController = sceneController;
      m_TurnSystem = turnSystem;
    }

    public void Execute()
    {
      var currPlayer = m_TurnSystem.IsPlayerTurn ? "Player" : "AI";
      m_SceneController.TriggerOnCallACommand(COMMAND_TEXT + currPlayer);
      m_TurnSystem.EndTurn();
    }
  }
}