using System;
using UnityEngine;
using TMPro;
using Tactics.Units;

namespace Tactics
{
  public class UIController : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI m_PlayerUnitsNumber = null;
    [SerializeField] private TextMeshProUGUI m_AIUnitsNumber = null;
    [SerializeField] private TextMeshProUGUI m_ActionsText = null;
    [SerializeField] private TextMeshProUGUI m_EndGameText = null;
    [SerializeField] private TextMeshProUGUI m_EndGameActionsText = null;

    private SceneController m_SceneController = null;

    private void Awake()
    {
      m_SceneController = FindObjectOfType<SceneController>();
      m_SceneController.OnPlayerUnitsNumberChanged += SetPlayerUnitsNumber;
      m_SceneController.OnAIUnitsNumberChanged += SetAIUnitsNumber;
      m_SceneController.OnCallACommand += SetActionsText;
      m_SceneController.OnEndGame += ShowEndGamePanel;
      m_SceneController.OnUnitKilled += SetUnitsNumber;
    }

    private void OnDestroy()
    {
      m_SceneController.OnPlayerUnitsNumberChanged -= SetPlayerUnitsNumber;
      m_SceneController.OnAIUnitsNumberChanged -= SetAIUnitsNumber;
      m_SceneController.OnCallACommand -= SetActionsText;
      m_SceneController.OnEndGame -= ShowEndGamePanel;
      m_SceneController.OnUnitKilled -= SetUnitsNumber;
    }

    private void SetPlayerUnitsNumber(object sender, SceneController.OnUnitNumberChangedEventArgs e)
    {
      m_PlayerUnitsNumber.text = e.UnitsNumber.ToString();
    }

    private void SetAIUnitsNumber(object sender, SceneController.OnUnitNumberChangedEventArgs e)
    {
      m_AIUnitsNumber.text = e.UnitsNumber.ToString();
    }

    private void SetActionsText(object sender, SceneController.OnCallACommandEventArgs e)
    {
      m_ActionsText.text = "> " + e.CommandText + "\n" + m_ActionsText.text;
    }

    private void ShowEndGamePanel(object sender, SceneController.OnEndGameEventArgs e)
    {
      m_EndGameText.text = e.EndGameText;
      m_EndGameActionsText.text = m_ActionsText.text;
      m_EndGameText.transform.parent.gameObject.SetActive(true);      
    }

    private void SetUnitsNumber(Unit unit)
    {
      if(unit.Settings.UseAI)
      {
        var number = Int32.Parse(m_AIUnitsNumber.text);
        --number;
        m_AIUnitsNumber.text = number.ToString();
      }
      else
      {
        var number = Int32.Parse(m_PlayerUnitsNumber.text);
        --number;
        m_PlayerUnitsNumber.text = number.ToString();
      }
    }
  }
}