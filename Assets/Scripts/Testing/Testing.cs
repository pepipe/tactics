using Tactics.Grid;
using UnityEngine;

namespace Tactics.Testing
{
  public class Testing : MonoBehaviour
  {
    public Material WalkableMaterial;
    public Material BlockerMaterial;

    private SceneController m_SceneController = null;
    private bool m_MousePressed = false;

    private void Awake()
    {
      m_SceneController = FindObjectOfType<SceneController>();
    }

    private void Update()
    {
      Vector3 mouseWorldPosition = Utils.GetMouseWorldPosition(Camera.main);
      ProcessLeftClick(mouseWorldPosition);
      ProcessRightClick(mouseWorldPosition);
    }

    private void ProcessLeftClick(Vector3 mouseWorldPosition)
    {
      if (Input.GetMouseButtonDown(0) && !m_MousePressed)
      {
        m_MousePressed = true;
        
      }
      else if (Input.GetMouseButtonUp(0))
        m_MousePressed = false;
    }

    private void ProcessRightClick(Vector3 mouseWorldPosition)
    {
      if (Input.GetMouseButtonDown(1) && !m_MousePressed)
      {
        m_MousePressed = true;
        ToggleNodeWalkable(mouseWorldPosition);
        //CreateGridSelectionNode(mouseWorldPosition);
      }
      else if (Input.GetMouseButtonUp(1))
        m_MousePressed = false;
    }

    private void ToggleNodeWalkable(Vector3 mouseWorldPosition)
    {
      var (x, y) = m_SceneController.Grid.GetXY(mouseWorldPosition);
      var gridObject = m_SceneController.Grid.GetGridObject(x, y);
      if (gridObject != null)
      {
        gridObject.SetWalkable(!gridObject.Node.IsWalkable);
        var mat = gridObject.Node.IsWalkable ? WalkableMaterial : BlockerMaterial;
        ChangeNodeVisual(gridObject, mat);
      }
    }

    private void ChangeNodeVisual(GridObject gridObject, Material mat)
    {
      gridObject.ChangeVisual(mat);
    }

    private void CreateGridSelectionNode(Vector3 mouseWorldPosition)
    {
      var gridObj = m_SceneController.Grid.GetGridObject(mouseWorldPosition);
      var selection = Instantiate(m_SceneController.MovementNodePrefab,
                                  gridObj.transform,
                                  true);
      selection.transform.position = gridObj.transform.position;
    }
  }
}