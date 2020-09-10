using UnityEngine;

namespace Tactics.Units
{

  [CreateAssetMenu(menuName = "Tactics/Unit Settings", order = 1, fileName = "Unit")]
  public class UnitSettings : ScriptableObject
  {
    [SerializeField]
    private Unit m_UnitPrefab = null;
    [SerializeField]
    private bool m_UseAI = false;
    [SerializeField]
    private string m_UnitName = "John Doe";
    [SerializeField]
    private float m_Speed = 10f;
    [SerializeField]
    private float m_Health = 10f;
    [SerializeField]
    private float m_AttackDamage = 2.5f;
    [SerializeField]
    private int m_AttackRange = 0;
    [SerializeField]
    private int m_MoveUnits = 3;

    public Unit UnitPrefab => m_UnitPrefab;
    public bool UseAI => m_UseAI;
    public string UnitName => m_UnitName;
    public float Speed => m_Speed;
    public float Health => m_Health;
    public float AttackDamage => m_AttackDamage;
    public int AttackRange => m_AttackRange;
    public int MoveUnits => m_MoveUnits;
  }
}