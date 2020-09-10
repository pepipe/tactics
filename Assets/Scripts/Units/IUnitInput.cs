using Tactics.Grid;
using UnityEngine;

namespace Tactics.Units
{
  public interface IUnitInput
  {
    bool AdquiredMovePosition { get; }
    bool CheckAttack { get; }    
    Vector3 MovePosition { get; }
    Unit Target { get; }

    void MoveInput(SubGrid subGrid);
    void AttackInput(SubGrid subGrid);
  }
}