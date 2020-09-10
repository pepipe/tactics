using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tactics.Units.States
{
  public class StateMachine : MonoBehaviour
  {
    public BaseState CurrenState { get; private set; }
    public event Action<BaseState> OnStateChanged;

    private Dictionary<Type, BaseState> m_AvailableStates;

    public void SetStates(Dictionary<Type, BaseState> states)
    {
      m_AvailableStates = states;
      CurrenState = m_AvailableStates.Values.First();
    }

    private void Update()
    {
      if(CurrenState == null)
      {
        CurrenState = m_AvailableStates.Values.First();
      }

      var nextState = CurrenState?.Tick();

      if(nextState != null && nextState != CurrenState?.GetType())
      {
        SwitchToNewState(nextState);
      }
    }

    private void SwitchToNewState(Type nextState)
    {
      CurrenState = m_AvailableStates[nextState];
      OnStateChanged?.Invoke(CurrenState);
    }
  }
}