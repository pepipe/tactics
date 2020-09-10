using System.Collections.Generic;
using Tactics.Units;
using Tactics.Units.States;
using UnityEngine;
using System.Linq;

namespace Tactics
{
  public class TurnSystem
  {
    public bool IsPlayerTurn => m_IsPlayerTurn;
    private bool m_IsPlayerTurn;
    public List<Unit> PlayerUnits => m_PlayerUnits;
    private List<Unit> m_PlayerUnits = new List<Unit>();
    public List<Unit> AIUnits => m_AIUnits;
    private List<Unit> m_AIUnits = new List<Unit>();

    private int m_CurrUnitIndex = 0;    
    private Material m_ActiveSelection;
    private Material m_PlayerSelection;
    private Material m_AISelection;

    public TurnSystem(bool isPlayerTurn,
                      Material ActiveSelection,
                      Material PlayerSelection,
                      Material AISelection)
    {
      m_IsPlayerTurn = isPlayerTurn;
      m_ActiveSelection = ActiveSelection;
      m_PlayerSelection = PlayerSelection;
      m_AISelection = AISelection;
    }

    public void AddUnit(Unit unit, bool isPlayerUnit)
    {
      if (isPlayerUnit)
        m_PlayerUnits.Add(unit);
      else
        m_AIUnits.Add(unit);
    }

    public Unit ActivateNextUnit()
    {
      InactiveCurrentUnit();
      var unitsList = m_IsPlayerTurn ? m_PlayerUnits : m_AIUnits;
      var possibleUnits = unitsList.Where(x => x.GetCurrentState() == typeof(IdleState) ||
                                                x.GetCurrentState() == typeof(ActiveState));

      if(possibleUnits.Count() == 0)
        return null;

      ++m_CurrUnitIndex;
      ValidateUpperIndex();

      var unit = unitsList[m_CurrUnitIndex];
      unit.IsActive = true;
      unit.SetSelection(m_ActiveSelection);
      return unit;
    }

    public Unit ActivatePreviousUnit()
    {
      InactiveCurrentUnit();

      --m_CurrUnitIndex;
      ValidateBottomIndex();

      var unit = m_IsPlayerTurn ? m_PlayerUnits[m_CurrUnitIndex] :
                                    m_AIUnits[m_CurrUnitIndex];
      unit.IsActive = true;
      unit.SetSelection(m_ActiveSelection);
      return unit;
    }

    public void EndTurn()
    {
      InactiveCurrentUnit();
      m_CurrUnitIndex = 0;
      if (m_IsPlayerTurn)
        foreach (var unit in m_PlayerUnits)
          unit.AdvanceState = true;
      else
        foreach (var unit in m_AIUnits)
          unit.AdvanceState = true;

      m_IsPlayerTurn = !m_IsPlayerTurn;
    }

    public int GetActivePlayerUnitsNumber()
    {
      return m_IsPlayerTurn ?
              GetPlayerUnitsNumber() :
              GetAIUnitsNumber();
    }

    public int GetPlayerUnitsNumber()
    {
      return m_PlayerUnits.Count;
    }

    public int GetAIUnitsNumber()
    {
      return m_AIUnits.Count;
    }

    public void KillUnit(Unit unit)
    {
      GameObject.Destroy(unit.gameObject);
      if (unit.Settings.UseAI)
        m_AIUnits.Remove(unit);
      else
        m_PlayerUnits.Remove(unit);
    }

    public bool StillHaveUnits(bool isAI)
    {
      var result = false;
      if (!isAI && m_PlayerUnits.Count > 0)
        result = true;
      else if (isAI && m_AIUnits.Count > 0)
        result = true;

      return result;
    }

    private void ValidateUpperIndex()
    {
      if (m_IsPlayerTurn && m_CurrUnitIndex == m_PlayerUnits.Count ||
                !m_IsPlayerTurn && m_CurrUnitIndex == m_AIUnits.Count)
        m_CurrUnitIndex = 0;
    }

    private void ValidateBottomIndex()
    {
      if (m_CurrUnitIndex < 0 && m_IsPlayerTurn)
        m_CurrUnitIndex = m_PlayerUnits.Count - 1;
      else if (m_CurrUnitIndex < 0 && !m_IsPlayerTurn)
        m_CurrUnitIndex = m_AIUnits.Count - 1;
    }

    private void InactiveCurrentUnit()
    {
      if (m_IsPlayerTurn && m_PlayerUnits.Count > 0)
      {
        m_PlayerUnits[m_CurrUnitIndex].SetSelection(m_PlayerSelection);
        m_PlayerUnits[m_CurrUnitIndex].IsActive = false;
      }
      else if(m_AIUnits.Count > 0)
      {
        m_AIUnits[m_CurrUnitIndex].SetSelection(m_AISelection);
        m_AIUnits[m_CurrUnitIndex].IsActive = false;
      }
    }
  }
 }