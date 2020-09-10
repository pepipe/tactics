using System.Collections.Generic;
using Tactics.Grid;
using Tactics.Units;
using UnityEngine;

namespace Tactics
{
  [CreateAssetMenu(menuName = "Tactics/Game Settings", order = 0, fileName = "GameSettings")]
  public class GameSettings : ScriptableObject
  {
    [System.Serializable]
    public class NumberUnits
    {
      public int Min;
      public int Max;

      public NumberUnits(int min, int max)
      {
        Min = min;
        Max = max;
      }
    }

    [Header("Grid Settings")]
    [SerializeField]
    private int m_GridWidth = 10;
    [SerializeField]
    private int m_GridHeight = 10;
    [SerializeField]
    private float m_GridCellSize = 10f;

    [Header("GamePlay Settings")]
    [SerializeField]
    [Tooltip("Number of Player units to be spawn. Random from min to max.")]
    private NumberUnits m_NumberPlayerUnits = new NumberUnits(2, 4);    
    [SerializeField]
    private List<UnitSettings> m_PlayerUnits = null;    
    [SerializeField]
    [Tooltip("Number of AI units to be spawn. Random from min to max.")]
    private NumberUnits m_NumberAIUnits = new NumberUnits(4, 8);
    [SerializeField]
    private List<UnitSettings> m_AIUnits = null;

    [Header("Assets References")]
    [SerializeField]
    private List<GridObject> m_GridObjects = null;
    [SerializeField]
    private Material m_UnitActiveSelection = null;
    [SerializeField]
    private Material m_PlayerSelection = null;
    [SerializeField]
    private Material m_AISelection = null;

    public int GridWidth => m_GridWidth;
    public int GridHeight => m_GridHeight;
    public float GridCellSize => m_GridCellSize;

    public Material UnitActiveSelection => m_UnitActiveSelection;
    public List<GridObject> GridObjects => m_GridObjects;
    public Material PlayerSelection => m_PlayerSelection;
    public NumberUnits NumberPlayerUnits => m_NumberPlayerUnits;
    public List<UnitSettings> PlayerUnits => m_PlayerUnits;
    public Material AISelection => m_AISelection;
    public NumberUnits NumberAIUnits => m_NumberAIUnits;    
    public List<UnitSettings> AIUnits => m_AIUnits;
  }
}