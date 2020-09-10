using System;
using UnityEngine;

namespace Tactics.Units.States
{
  public abstract class BaseState
  {
    protected GameObject m_GameObject;
    protected Transform m_Trasform;

    public BaseState(GameObject gameObject)
    {
      m_GameObject = gameObject;
      m_Trasform = gameObject.transform;
    }

    public abstract Type Tick();
  }
}