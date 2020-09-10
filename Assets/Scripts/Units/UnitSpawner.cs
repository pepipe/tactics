using System.Collections.Generic;
using System.Linq;
using Tactics.Grid;
using UnityEngine;

namespace Tactics.Units
{
  public class UnitSpawner
  {
    private GameSettings m_Settings;
    private Transform m_UnitParent;
    private TurnSystem m_TurnSystem;
    private Grid<GridObject> m_Grid;

    public UnitSpawner(GameSettings settings, Transform parent, TurnSystem turnSystem, Grid<GridObject> grid)
    {
      m_Settings = settings;
      m_UnitParent = parent;
      m_TurnSystem = turnSystem;
      m_Grid = grid;
    }

    public void SpawnUnits()
    {
      var gridObjects = m_Grid.ToList();
      var unitsNumber = Random.Range(m_Settings.NumberPlayerUnits.Min, m_Settings.NumberPlayerUnits.Max + 1);
      for (int i = 0; i < unitsNumber; ++i)
      {
        var unitSettings = m_Settings.PlayerUnits[Random.Range(0, m_Settings.PlayerUnits.Count)];
        var unit = GameObject.Instantiate(unitSettings.UnitPrefab, m_UnitParent, false);
        unit.InitUnit(unitSettings, i);
        SetInitialUnitPosition(unit, gridObjects);
        unit.SetSelection(m_Settings.PlayerSelection);
        m_TurnSystem.AddUnit(unit, true);
      }

      unitsNumber = Random.Range(m_Settings.NumberAIUnits.Min, m_Settings.NumberAIUnits.Max + 1);
      for (int i = 0; i < unitsNumber; ++i)
      {
        var unitSettings = m_Settings.AIUnits[Random.Range(0, m_Settings.AIUnits.Count)];
        var unit = GameObject.Instantiate(unitSettings.UnitPrefab, m_UnitParent, false);
        unit.InitUnit(unitSettings, i);
        SetInitialUnitPosition(unit, gridObjects);
        unit.SetSelection(m_Settings.AISelection);
        m_TurnSystem.AddUnit(unit, false);
      }
    }

    private void SetInitialUnitPosition(Unit unit, List<GridObject> gridObjects, int x = -1, int y = -1)
    {
      var gridPos = GetSpawnPosition(gridObjects, x, y);
      var cellSize = m_Grid.GridCellSize;
      var nodePos = m_Grid.GetWorldPosition(gridPos.x, gridPos.y);
      unit.transform.position = new Vector3(nodePos.x + cellSize / 2f,
                                    nodePos.y + cellSize / 2f,
                                    unit.transform.position.z);
      m_Grid.GetGridObject(gridPos.x, gridPos.y).SetWalkable(false);
      unit.SetGridPos(gridPos);
      //var (x, y) = m_Grid.GetXY(nodePos);
      //Debug.Log(x + " | " + y);
    }

    private (int x, int y) GetSpawnPosition(List<GridObject> gridObjects, int gridX, int gridY)
    {
      if(gridX == -1 || gridY == -1)
      {
        var availableObjects = gridObjects.Where(x => x.Node.IsWalkable);
        var obj = availableObjects.ElementAt(Random.Range(0, availableObjects.Count()));
        gridX = obj.Node.XPos;
        gridY = obj.Node.YPos;
      }

      return (gridX, gridY);
    }
  }
}