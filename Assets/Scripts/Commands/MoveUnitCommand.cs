using Tactics.Units;
using UnityEngine;

namespace Tactics.Commands
{
  public class MoveUnitCommand : ICommand
  {
    private Unit m_Unit;
    private Vector3 m_PreviousPosition;
    private Vector3 m_TargetPosition;

    private const string COMMAND_TEXT = "Move Unit ";

    public MoveUnitCommand(Unit unit, Vector3 targetPosition)
    {
      m_Unit = unit;
      m_TargetPosition = targetPosition;
    }

    public void Execute()
    {
      m_PreviousPosition = m_Unit.transform.localPosition;
      var (playerX, playerY) = m_Unit.SceneController.Grid.GetXY(m_Unit.transform.position);
      var (targetX, targetY) = m_Unit.SceneController.Grid.GetXY(m_TargetPosition);

      //Debug.Log("Grid Player pos:" + playerX + "," + playerY);
      //Debug.Log("Grid Target pos:" + targetX + "," + targetY);

      m_Unit.GoToPosition(m_Unit.SceneController.GridPathFinding.GetVector3Path(
                        m_Unit.SceneController.GetPath(playerX, playerY, targetX, targetY)));

      m_Unit.SceneController.TriggerOnCallACommand(COMMAND_TEXT + m_Unit.name + " to ["
                                                  + targetX + "," + targetY + "]");
    }
  }
}