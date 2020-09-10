using System;
using System.Collections.Generic;
using Tactics.Units.States;
using UnityEngine;

namespace Tactics.Units
{
  [RequireComponent(typeof(StateMachine))]
  public class Unit : MonoBehaviour
  {
    public bool IsActive = false;

    [SerializeField]
    private SpriteRenderer m_Selection = null;
    [SerializeField]
    private GameObject m_HealthBar = null;
    [SerializeField]
    private ParticleSystem m_BloodFX = null;

    //StateMachine flags
    public bool AdvanceState { get; set; }

    public SceneController SceneController => m_SceneController;
    private SceneController m_SceneController;
    public List<Vector3> Path => m_Path;
    private List<Vector3> m_Path;
    public UnitSettings Settings => m_Settings;
    private UnitSettings m_Settings;
    public (int X, int Y) GridPos => m_GridPos;
    private (int X, int Y) m_GridPos;

    public IUnitInput UnitInput => m_UnitInput;
    private IUnitInput m_UnitInput;
    public float Health => m_Health;
    private float m_Health;

    private Animator m_Anim;
    private Quaternion m_InitialRotation;
    private float m_ZPos;
    private int m_CurrentPathIndex;    
    private StateMachine m_StateMachine;

    public void GoToPosition(List<Vector3> path)
    {
      m_Anim = GetComponent<Animator>();
      m_Path = path;
      m_CurrentPathIndex = 0;
      m_SceneController.Grid.GetGridObject(transform.position).SetWalkable(true);
      if (m_Path != null && m_Path.Count > 1)
        m_Path.RemoveAt(0);
    }

    public void InitUnit(UnitSettings settings, int number)
    {
      m_Settings = settings;
      name = m_Settings.UseAI ? "AI_" : "Player_";
      name += number + "_" + m_Settings.name;
      IsActive = false;
      m_ZPos = transform.localPosition.z;
      m_InitialRotation = transform.rotation;
      m_Health = m_Settings.Health;

      m_UnitInput = m_Settings.UseAI ?
                  new UnitAiInput(this) as IUnitInput :
                  new UnitPlayerInput(this);
    }

    public void SetGridPos((int x, int y) gridPos)
    {
      m_GridPos = gridPos;
    }

    public void SetSelection(Material mat)
    {
      m_Selection.material = mat;
    }

    public void TakeDamage(float damage)
    {
      m_Health -= damage;
      m_HealthBar.transform.localScale = new Vector3(m_Health / m_Settings.Health,
                                                      m_HealthBar.transform.localScale.y,
                                                      m_HealthBar.transform.localScale.z);

      m_BloodFX.Play();

      if (m_Health <= 0)
        m_SceneController.TriggerOnUnitKilled(this);
    }

    public Type GetCurrentState()
    {
      return m_StateMachine.CurrenState.GetType();
    }

    private void Awake()
    {
      m_SceneController = FindObjectOfType<SceneController>();
      m_StateMachine = GetComponent<StateMachine>();      
      InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
      var states = new Dictionary<Type, BaseState>()
      {
        {typeof(IdleState), new IdleState(this) },
        {typeof(ActiveState), new ActiveState(this) },
        {typeof(MovingState), new MovingState(this) },
        {typeof(AttackState), new AttackState(this) },
        {typeof(WaitingEndTurnState), new WaitingEndTurnState(this) }
      };

      m_StateMachine.SetStates(states);
    }

    public void Move()
    {
      if (m_Path != null)
      {
        Vector3 targetPosition = m_Path[m_CurrentPathIndex];
        targetPosition.z = m_ZPos;
        //Debug.Log(targetPosition.x + "," + targetPosition.y);
        
        //moving towards a path position
        if (Vector3.Distance(transform.localPosition, targetPosition) > 1f)
        {
          Vector3 moveDir = (targetPosition - transform.localPosition).normalized;
          SetMoveAnimation(moveDir, m_Settings.Speed);
          transform.localPosition = transform.localPosition + moveDir * m_Settings.Speed * Time.deltaTime;
        }
        else//we get there then we need to get the next path position
        {          
          ++m_CurrentPathIndex;
          if (m_CurrentPathIndex >= m_Path.Count)
            StopMoving(targetPosition);
        }
      }
    }

    public void StopMoving(Vector3 finalPosition)
    {
      //Debug.Log("Stop Moving");
      m_Path = null;      
      transform.localPosition = finalPosition;//snap to final
      m_SceneController.Grid.GetGridObject(transform.position).SetWalkable(false);
      SetGridPos(m_SceneController.Grid.GetXY(transform.position));
      SetMoveAnimation(m_InitialRotation, 0);
    }

    private void SetMoveAnimation(Vector3 moveDir, float speed)
    {
      transform.rotation = Quaternion.LookRotation(moveDir, -Vector3.forward);
      m_Anim.SetFloat("Speed_f", speed);
    }

    private void SetMoveAnimation(Quaternion rotation, float speed)
    {
      transform.rotation = rotation;
      m_Anim.SetFloat("Speed_f", speed);
    }
  }
}