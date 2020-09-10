using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Commands
{
  public class CommandManager : MonoBehaviour
  {
    private List<ICommand> m_CommandBuffer = new List<ICommand>();

    //create a method to add commands to the command buffer
    public void AddCommand(ICommand command)
    {
      m_CommandBuffer.Add(command);
      command.Execute();
    }

    //Reset - clear the command buffer
    public void Reset()
    {
      m_CommandBuffer.Clear();
    }
  }
}