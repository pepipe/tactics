using System;
using System.Collections.Generic;
using Tactics.Commands;
using Tactics.Grid;
using Tactics.Paths;
using Tactics.Units;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tactics
{  
  [RequireComponent(typeof(CommandManager))]
  public class SceneController : MonoBehaviour
  {
    public event EventHandler<OnUnitNumberChangedEventArgs> OnPlayerUnitsNumberChanged;
    public event EventHandler<OnUnitNumberChangedEventArgs> OnAIUnitsNumberChanged;
    public event EventHandler<OnCallACommandEventArgs> OnCallACommand;
    public event EventHandler<OnEndGameEventArgs> OnEndGame;
    public event OnUnityKilledDelegate OnUnitKilled;

    public class OnUnitNumberChangedEventArgs : EventArgs { public int UnitsNumber; }
    public class OnCallACommandEventArgs : EventArgs { public string CommandText; }
    public class OnEndGameEventArgs : EventArgs { public string EndGameText; }
    public delegate void OnUnityKilledDelegate(Unit unit);

    [SerializeField]
    private GameSettings m_GameSettings = null;
    [SerializeField]
    private GameObject m_GridParent = null;
    [SerializeField]
    private GameObject m_UnitParent = null;
    [SerializeField]
    private GameObject m_MovementNodePrefab = null;
    [SerializeField]
    private GameObject m_AttackNodePrefab = null;
    [SerializeField]
    private bool m_ShowGridDebug = true;

    public CommandManager CommandProcessor { get; private set; }

    public GameObject MovementNodePrefab => m_MovementNodePrefab;
    public GameObject AttackNodePrefab => m_AttackNodePrefab;

    public Grid<GridObject> Grid { get => m_Grid; }
    private Grid<GridObject> m_Grid;    
    private Unit m_ActiveUnit;
    public PathFinding GridPathFinding { get => m_PathFinding; }
    private PathFinding m_PathFinding;
    public TurnSystem TurnSystem => m_TurnSystem;
    private TurnSystem m_TurnSystem;
    private UnitSpawner m_UnitSpawner;

    public List<PathNode> GetPath(int startX, int startY, int endX, int endY)
    {
      List<PathNode> path = m_PathFinding.FindPath(startX, startY, endX, endY);
      //m_Path.PrintPath(path);
      if (path != null)
      {
        for (int i = 0; i < path.Count - 1; i++)
        {
          var startPos = m_Grid.GetWorldPosition(path[i].XPos, path[i].YPos);
          var endPos = m_Grid.GetWorldPosition(path[i + 1].XPos, path[i + 1].YPos);
          Debug.DrawLine(new Vector3(startPos.x + m_Grid.GridCellSize / 2,
                                      startPos.y + m_Grid.GridCellSize / 2,
                                      startPos.z),
                          new Vector3(endPos.x + m_Grid.GridCellSize / 2,
                                      endPos.y + m_Grid.GridCellSize / 2,
                                      endPos.z),
                          Color.green,
                          5f);
        }
        return path;
      }

      return null;
    }
    public void BuildMovementNodes(int startX, int startY, int endX, int endY)
    {
    }

    public void ActivateNextUnitUI()
    {
      if (TurnSystem.IsPlayerTurn)
        ActivateNextUnit();
    }

    public void ActivateNextUnit()
    {
      var activateUnitCommand = new ActivateNextUnitCommand(this, m_TurnSystem);
      CommandProcessor.AddCommand(activateUnitCommand);
    }

    public void EndTurnUI()
    {
      if (TurnSystem.IsPlayerTurn)
        EndTurn();
    }

    public void EndTurn()
    {
      var endTurnCommand = new EndTurnCommand(this, m_TurnSystem);
      CommandProcessor.AddCommand(endTurnCommand);

      var activateUnitCommand = new ActivateNextUnitCommand(this, m_TurnSystem);
      CommandProcessor.AddCommand(activateUnitCommand);
    }

    public void RestartScene()
    {
      SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void SetActiveUnit(Unit activeUnit)
    {
      m_ActiveUnit = activeUnit;
    }

    public void TriggerPlayerUnitsNumberChanged(int unitsNumber)
    {
      OnPlayerUnitsNumberChanged?.Invoke(this, new OnUnitNumberChangedEventArgs { UnitsNumber =  unitsNumber});
    }
    public void TriggerAIUnitsNumberChanged(int unitsNumber)
    {
      OnAIUnitsNumberChanged?.Invoke(this, new OnUnitNumberChangedEventArgs { UnitsNumber = unitsNumber });
    }

    public void TriggerOnCallACommand(string commandText)
    {
      OnCallACommand?.Invoke(this, new OnCallACommandEventArgs { CommandText = commandText });
    }

    public void TriggerOnUnitKilled(Unit unit)
    {
      m_TurnSystem.KillUnit(unit);
      m_Grid.GetGridObject(unit.GridPos.X, unit.GridPos.Y).SetWalkable(true);
      OnUnitKilled?.Invoke(unit);
      if (!m_TurnSystem.StillHaveUnits(unit.Settings.UseAI))
      {
        var endGameText = unit.Settings.UseAI ? "You Won!" : "You Lose";
        OnEndGame?.Invoke(this, new OnEndGameEventArgs { EndGameText = endGameText });
      }
    }

    private void Awake()
    {
      CommandProcessor = GetComponent<CommandManager>();
    }

    private void Start()
    {
      m_Grid = new Grid<GridObject>(m_GameSettings.GridWidth,
                                      m_GameSettings.GridHeight,
                                      m_GameSettings.GridCellSize,
                                      m_GridParent.transform,
                                      (Grid<GridObject> g, int x, int y, bool debug) => {
                                        var obj = Instantiate(m_GameSettings.GridObjects[0], m_GridParent.transform, false);
                                        return obj.Init(g, x, y, m_ShowGridDebug);
                                      });

      m_TurnSystem = new TurnSystem(true, 
                            m_GameSettings.UnitActiveSelection,
                            m_GameSettings.PlayerSelection,
                            m_GameSettings.AISelection);
      m_PathFinding = new PathFinding(m_Grid);

      m_UnitSpawner = new UnitSpawner(m_GameSettings, m_UnitParent.transform, m_TurnSystem, m_Grid);
      m_UnitSpawner.SpawnUnits();      
      TriggerPlayerUnitsNumberChanged(m_TurnSystem.GetPlayerUnitsNumber());
      TriggerAIUnitsNumberChanged(m_TurnSystem.GetAIUnitsNumber());
      ActivateNextUnit();
    }

    private void Update()
    {
      if (TurnSystem.IsPlayerTurn)
      {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
          ActivateNextUnit();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
          EndTurn();
        }
      }
    }
  }
}